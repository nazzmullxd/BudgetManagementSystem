using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

namespace Business.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<CurrencyService> logger)
            : base(userRepository, auditService)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public void AddCurrency(string code, string name, decimal exchangeRateToBase)
        {
            _logger.LogInformation("Adding currency: {Code}", code);

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            {
                _logger.LogError("AddCurrency failed: Currency code and name are required.");
                throw new ArgumentException("Currency code and name are required.");
            }

            if (exchangeRateToBase <= 0)
            {
                _logger.LogError("AddCurrency failed: Exchange rate must be greater than zero for currency {Code}", code);
                throw new ArgumentException("Exchange rate must be greater than zero.");
            }

            var existingCurrency = _currencyRepository.GetAll()
                .FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            if (existingCurrency != null)
            {
                _logger.LogError("AddCurrency failed: Currency {Code} already exists.", code);
                throw new ArgumentException("A currency with this code already exists.");
            }

            var currency = new Currency
            {
                Code = code,
                Name = name,
                ExchangeRateToBase = exchangeRateToBase,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _currencyRepository.Add(currency);
            _logger.LogInformation("Currency {Code} added successfully.", code);
        }

        public decimal ConvertAmount(decimal amount, string fromCurrencyId, string toCurrencyId)
        {
            _logger.LogInformation("Converting amount {Amount} from currency {FromCurrencyId} to {ToCurrencyId}", amount, fromCurrencyId, toCurrencyId);

            if (amount < 0)
            {
                _logger.LogError("ConvertAmount failed: Amount cannot be negative.");
                throw new ArgumentException("Amount cannot be negative.");
            }

            if (string.IsNullOrWhiteSpace(fromCurrencyId) || string.IsNullOrWhiteSpace(toCurrencyId))
            {
                _logger.LogError("ConvertAmount failed: Currency IDs are required.");
                throw new ArgumentException("Currency IDs are required.");
            }

            var fromCurrency = _currencyRepository.GetById(fromCurrencyId);
            var toCurrency = _currencyRepository.GetById(toCurrencyId);

            if (fromCurrency == null || toCurrency == null)
            {
                _logger.LogError("ConvertAmount failed: Invalid currency ID. From: {FromCurrencyId}, To: {ToCurrencyId}", fromCurrencyId, toCurrencyId);
                throw new ArgumentException("Invalid currency ID.");
            }

            decimal amountInBase = amount * fromCurrency.ExchangeRateToBase;
            decimal convertedAmount = amountInBase / toCurrency.ExchangeRateToBase;

            var result = Math.Round(convertedAmount, 2);
            _logger.LogInformation("Converted {Amount} from {FromCurrency} to {ToCurrency}: {Result}", amount, fromCurrency.Code, toCurrency.Code, result);
            return result;
        }

        public List<Currency> GetAllCurrencies()
        {
            _logger.LogInformation("Retrieving all currencies");

            var currencies = _currencyRepository.GetAll();
            _logger.LogInformation("Retrieved {Count} currencies", currencies.Count);
            return currencies;
        }
    }
}