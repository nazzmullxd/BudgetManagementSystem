using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface ITagRepository
    {
        void Add(Tag tag);
        List<Tag> GetByUserId(string userId);
        Tag GetById(string tagId);
    }
}