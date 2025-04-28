using Database.Model;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface ICurrencyRepository
    {
        void Add(Currency currency);
        Currency GetById(string currencyId);
        List<Currency> GetAll();
    }
}