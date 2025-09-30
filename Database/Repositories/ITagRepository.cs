using Database.Model;

namespace Database.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        List<Tag>? GetByUserId(string userId);
        Tag? GetById(string tagId);
        void Update(Tag tag);
        void Delete(Tag tag);
    }
}