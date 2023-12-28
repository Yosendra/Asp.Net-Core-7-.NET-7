using ServiceContracts;

namespace CRUDTests;

public class CountryServiceTest
{
    private readonly ICountryService _countryService;

    public CountryServiceTest(ICountryService countryService)
    {
        _countryService = countryService;
    }
}
