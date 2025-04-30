using Database.Model;
using BudgetManagementSystem.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Interfeces;

namespace Business.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly ITransactionTagRepository _transactionTagRepository;
        private readonly ILogger<TagService> _logger;

        public TagService(
            ITagRepository tagRepository,
            ITransactionTagRepository transactionTagRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<TagService> logger)
            : base(userRepository, auditService)
        {
            _tagRepository = tagRepository;
            _transactionTagRepository = transactionTagRepository;
            _logger = logger;
        }

        public void CreateTag(string userId, string tagName)
        {
            _logger.LogInformation("Creating tag for user {UserId}: {TagName}", userId, tagName);

            ValidateUser(userId);

            if (string.IsNullOrWhiteSpace(tagName))
            {
                _logger.LogError("CreateTag failed: Tag name is required for user {UserId}", userId);
                throw new ArgumentException("Tag name is required.");
            }

            var existingTag = _tagRepository.GetByUserId(userId)
                .FirstOrDefault(t => t.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (existingTag != null)
            {
                _logger.LogError("CreateTag failed: Tag {TagName} already exists for user {UserId}", tagName, userId);
                throw new ArgumentException("A tag with this name already exists for the user.");
            }

            var tag = new Tag
            {
                UserId = userId,
                TagName = tagName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _tagRepository.Add(tag);
            LogAction(_logger, userId, "Tag Created", $"Tag {tagName} created");
        }

        public void UpdateTag(string tagId, string tagName)
        {
            _logger.LogInformation("Updating tag {TagId}", tagId);

            if (string.IsNullOrWhiteSpace(tagId))
            {
                _logger.LogError("UpdateTag failed: Tag ID is required.");
                throw new ArgumentException("Tag ID is required.");
            }

            if (string.IsNullOrWhiteSpace(tagName))
            {
                _logger.LogError("UpdateTag failed: Tag name is required for tag {TagId}", tagId);
                throw new ArgumentException("Tag name is required.");
            }

            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
            {
                _logger.LogError("UpdateTag failed: Tag {TagId} not found.", tagId);
                throw new ArgumentException("Tag not found.");
            }

            var existingTag = _tagRepository.GetByUserId(tag.UserId)
                .FirstOrDefault(t => t.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase) && t.TagId != tagId);

            if (existingTag != null)
            {
                _logger.LogError("UpdateTag failed: Tag {TagName} already exists for user {UserId}", tagName, tag.UserId);
                throw new ArgumentException("A tag with this name already exists for the user.");
            }

            tag.TagName = tagName;
            tag.UpdatedAt = DateTime.Now;

            _tagRepository.Update(tag);
            LogAction(_logger, tag.UserId, "Tag Updated", $"Tag {tagId} updated to {tagName}");
        }

        public void DeleteTag(string tagId)
        {
            _logger.LogInformation("Deleting tag {TagId}", tagId);

            if (string.IsNullOrWhiteSpace(tagId))
            {
                _logger.LogError("DeleteTag failed: Tag ID is required.");
                throw new ArgumentException("Tag ID is required.");
            }

            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
            {
                _logger.LogError("DeleteTag failed: Tag {TagId} not found.", tagId);
                throw new ArgumentException("Tag not found.");
            }

            var transactionTags = _transactionTagRepository.GetByTagId(tagId);
            foreach (var transactionTag in transactionTags)
            {
                _transactionTagRepository.Delete(transactionTag.TransactionTagId);
            }

            _tagRepository.Delete(tag);
            LogAction(_logger, tag.UserId, "Tag Deleted", $"Tag {tagId} deleted");
        }

        public List<Tag> GetTagsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving tags for user {UserId}", userId);

            ValidateUser(userId);

            var tags = _tagRepository.GetByUserId(userId);
            _logger.LogInformation("Retrieved {Count} tags for user {UserId}", tags.Count, userId);
            return tags;
        }
    }
}