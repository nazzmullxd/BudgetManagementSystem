using Database.Model;

namespace Database.Repositories
{
    public interface ICurrencyRepository
    {
        void Add(Currency currency);
        Currency? GetById(string currencyId);
        List<Currency>? GetAll();
    }
}