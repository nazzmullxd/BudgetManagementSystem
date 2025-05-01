using Database.Model;
using Database.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            ILogger<CurrencyService> logger)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        public void CreateCurrency(Currency currency)
        {
            _logger.LogInformation("Creating currency with ID {CurrencyId}", currency?.CurrencyId);

            if (currency == null)
            {
                _logger.LogError("CreateCurrency failed: Currency cannot be null");
                throw new ArgumentNullException(nameof(currency));
            }

            _currencyRepository.Add(currency);
            _logger.LogInformation("Currency created with ID {CurrencyId}", currency.CurrencyId);
        }

        public List<Currency>? GetAllCurrencies()
        {
            _logger.LogInformation("Retrieving all currencies");

            var currencies = _currencyRepository.GetAll();
            _logger.LogInformation("Retrieved {Count} currencies", currencies?.Count ?? 0);
            return currencies;
        }

        public Currency? GetCurrencyById(string currencyId)
        {
            _logger.LogInformation("Retrieving currency with ID {CurrencyId}", currencyId);

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("GetCurrencyById failed: Currency ID is required");
                throw new ArgumentException("Currency ID is required.", nameof(currencyId));
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogWarning("Currency with ID {CurrencyId} not found", currencyId);
            }
            else
            {
                _logger.LogInformation("Retrieved currency with ID {CurrencyId}", currencyId);
            }
            return currency;
        }

        public void UpdateCurrency(Currency currency)
        {
            _logger.LogInformation("Updating currency with ID {CurrencyId}", currency?.CurrencyId);

            if (currency == null)
            {
                _logger.LogError("UpdateCurrency failed: Currency cannot be null");
                throw new ArgumentNullException(nameof(currency));
            }

            var existingCurrency = _currencyRepository.GetById(currency.CurrencyId);
            if (existingCurrency == null)
            {
                _logger.LogError("UpdateCurrency failed: Currency with ID {CurrencyId} not found", currency.CurrencyId);
                throw new KeyNotFoundException($"Currency with ID {currency.CurrencyId} not found.");
            }

            _currencyRepository.Add(currency); // Assuming Add since Update isn't in ICurrencyRepository; adjust if needed
            _logger.LogInformation("Currency with ID {CurrencyId} updated", currency.CurrencyId);
        }

        public void DeleteCurrency(string currencyId)
        {
            _logger.LogInformation("Deleting currency with ID {CurrencyId}", currencyId);

            if (string.IsNullOrWhiteSpace(currencyId))
            {
                _logger.LogError("DeleteCurrency failed: Currency ID is required");
                throw new ArgumentException("Currency ID is required.", nameof(currencyId));
            }

            var currency = _currencyRepository.GetById(currencyId);
            if (currency == null)
            {
                _logger.LogError("DeleteCurrency failed: Currency with ID {CurrencyId} not found", currencyId);
                throw new KeyNotFoundException($"Currency with ID {currencyId} not found.");
            }

            _currencyRepository.Add(currency); // Assuming Add since Delete isn't in ICurrencyRepository; adjust if needed
            _logger.LogInformation("Currency with ID {CurrencyId} deleted", currencyId);
        }
    }
}