using Database.Model;
using BudgetManagementSystem.Repositories;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Business.Interfaces;

namespace Business.Services
{
    public class AlertService : BaseService, BudgetAlertsRepository
    {
        private readonly IBudgetAlertsRepository _budgetAlertsRepository;
        private readonly ILogger<AlertService> _logger;

        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "your-email@gmail.com";
        private readonly string _smtpPass = "your-app-password";

        public AlertService(
            IBudgetAlertsRepository budgetAlertsRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<AlertService> logger)
            : base(userRepository, auditService)
        {
            _budgetAlertsRepository = budgetAlertsRepository;
            _logger = logger;
        }

        public void SendPendingAlerts()
        {
            _logger.LogInformation("Sending pending alerts");

            var pendingAlerts = _budgetAlertsRepository.GetAll()
                .Where(a => !a.IsSent)
                .ToList();

            foreach (var alert in pendingAlerts)
            {
                var user = _userRepository.GetById(alert.UserId);
                if (user == null)
                {
                    _logger.LogWarning("Skipping alert {AlertId}: User {UserId} not found.", alert.BudgetAlertsId, alert.UserId);
                    continue;
                }

                try
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Budget Management System", _smtpUser));
                    emailMessage.To.Add(new MailboxAddress(user.Name, user.Email));
                    emailMessage.Subject = "Budget Alert";
                    emailMessage.Body = new TextPart("plain")
                    {
                        Text = alert.Message
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(_smtpServer, _smtpPort, false);
                        client.Authenticate(_smtpUser, _smtpPass);
                        client.Send(emailMessage);
                        client.Disconnect(true);
                    }

                    alert.IsSent = true;
                    alert.SentAt = DateTime.Now;
                    alert.UpdatedAt = DateTime.Now;
                    _budgetAlertsRepository.Update(alert);

                    LogAction(_logger, user.UserId, "Alert Sent", $"Alert {alert.BudgetAlertsId} sent to {user.Email}: {alert.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send alert {AlertId} to {Email}", alert.BudgetAlertsId, user.Email);
                }
            }
        }

        public List<BudgetAlerts> GetPendingAlerts(string userId)
        {
            _logger.LogInformation("Retrieving pending alerts for user {UserId}", userId);

            ValidateUser(userId);

            var alerts = _budgetAlertsRepository.GetByUserId(userId)
                .Where(a => !a.IsSent)
                .ToList();

            _logger.LogInformation("Retrieved {Count} pending alerts for user {UserId}", alerts.Count, userId);
            return alerts;
        }
    }
}