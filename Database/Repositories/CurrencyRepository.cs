using Database.Context;
using Database.Model;


namespace Database.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly BudgetManagementContext _context;

        public CurrencyRepository(BudgetManagementContext context)
        {
            _context = context;
        }

        public void Add(Currency currency)
        {
            _context.Currencies.Add(currency);
            _context.SaveChanges();
        }

        public Currency? GetById(string currencyId)
        {
            return _context.Currencies
                .FirstOrDefault(c => c.CurrencyId == currencyId);
        }

        public List<Currency>? GetAll()
        {
            return _context.Currencies.ToList();
        }
    }
}