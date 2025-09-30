using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IAuditService _auditService;
        private readonly ILogger<TagService> _logger;

        public TagService(
            ITagRepository tagRepository,
            IUserRepository userRepository,
            IAuditService auditService,
            ILogger<TagService> logger)
            : base(userRepository)
        {
            _tagRepository = tagRepository;
            _auditService = auditService;
            _logger = logger;
        }

        public void CreateTag(Tag tag)
        {
            _logger.LogInformation("Creating tag for user {UserId}", tag?.UserId);

            if (tag == null)
            {
                _logger.LogError("CreateTag failed: Tag cannot be null");
                throw new ArgumentNullException(nameof(tag));
            }

            ValidateUser(tag.UserId);

            _tagRepository.Add(tag);
            _auditService.LogAction(tag.UserId, "CreateTag", $"Tag created with ID {tag.TagId}");
            _logger.LogInformation("Tag created for user {UserId} with ID {TagId}", tag.UserId, tag.TagId);
        }

        public List<Tag>? GetTagsByUserId(string userId)
        {
            _logger.LogInformation("Retrieving tags for user {UserId}", userId);

            ValidateUser(userId);

            var tags = _tagRepository.GetByUserId(userId);
            _auditService.LogAction(userId, "GetTagsByUserId", $"Retrieved {tags?.Count ?? 0} tags");
            _logger.LogInformation("Retrieved {Count} tags for user {UserId}", tags?.Count ?? 0, userId);
            return tags;
        }

        public Tag? GetTagById(string tagId)
        {
            _logger.LogInformation("Retrieving tag with ID {TagId}", tagId);

            if (string.IsNullOrWhiteSpace(tagId))
            {
                _logger.LogError("GetTagById failed: Tag ID is required");
                throw new ArgumentException("Tag ID is required.", nameof(tagId));
            }

            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
            {
                _logger.LogWarning("Tag with ID {TagId} not found", tagId);
            }
            else
            {
                _logger.LogInformation("Retrieved tag with ID {TagId}", tagId);
                _auditService.LogAction(tag.UserId, "GetTagById", $"Retrieved tag with ID {tagId}");
            }
            return tag;
        }

        public void UpdateTag(Tag tag)
        {
            _logger.LogInformation("Updating tag with ID {TagId}", tag?.TagId);

            if (tag == null)
            {
                _logger.LogError("UpdateTag failed: Tag cannot be null");
                throw new ArgumentNullException(nameof(tag));
            }

            ValidateUser(tag.UserId);

            var existingTag = _tagRepository.GetById(tag.TagId);
            if (existingTag == null)
            {
                _logger.LogError("UpdateTag failed: Tag with ID {TagId} not found for user {UserId}", tag.TagId, tag.UserId);
                throw new KeyNotFoundException($"Tag with ID {tag.TagId} not found.");
            }

            _tagRepository.Update(tag);
            _auditService.LogAction(tag.UserId, "UpdateTag", $"Tag updated with ID {tag.TagId}");
            _logger.LogInformation("Tag with ID {TagId} updated", tag.TagId);
        }

        public void DeleteTag(string tagId)
        {
            _logger.LogInformation("Deleting tag with ID {TagId}", tagId);

            if (string.IsNullOrWhiteSpace(tagId))
            {
                _logger.LogError("DeleteTag failed: Tag ID is required");
                throw new ArgumentException("Tag ID is required.", nameof(tagId));
            }

            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
            {
                _logger.LogError("DeleteTag failed: Tag with ID {TagId} not found", tagId);
                throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
            }

            ValidateUser(tag.UserId);

            _tagRepository.Delete(tag);
            _auditService.LogAction(tag.UserId, "DeleteTag", $"Tag deleted with ID {tagId}");
            _logger.LogInformation("Tag with ID {TagId} deleted", tagId);
        }
    }
}