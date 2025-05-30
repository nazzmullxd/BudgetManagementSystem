﻿using Database.Model;

namespace Database.Repositories
{
    public interface IDuesManagementRepository
    {
        void Add(DuesManagement due);
        List<DuesManagement>? GetByUserId(string userId);
        DuesManagement? GetById(string dueId);
        void Update(DuesManagement due);
        void Delete(DuesManagement due);
    }
}