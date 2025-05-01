using Database.Model;

namespace Business.Services
{
    public interface ICurrencyService
    {
        void CreateCurrency(Currency currency);
        List<Currency>? GetAllCurrencies();
        Currency? GetCurrencyById(string currencyId);
        void UpdateCurrency(Currency currency);
        void DeleteCurrency(string currencyId);
    }
}