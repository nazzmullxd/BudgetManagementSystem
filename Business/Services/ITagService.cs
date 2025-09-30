using Database.Model;

namespace Business.Services
{
    public interface ITagService
    {
        void CreateTag(Tag tag);
        List<Tag>? GetTagsByUserId(string userId);
        Tag? GetTagById(string tagId);
        void UpdateTag(Tag tag);
        void DeleteTag(string tagId);
    }
}