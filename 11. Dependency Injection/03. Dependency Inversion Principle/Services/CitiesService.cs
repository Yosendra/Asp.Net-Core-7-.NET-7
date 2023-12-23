using ServiceContracts;

namespace Services;

public class CitiesService : ICitiesService     // Inherit the interface, forcing service class to implement the contract
{
    private List<string> _cities;

    public CitiesService()
    {
        _cities = new List<string>()
        {
            "London", "Paris", "New York", "Tokyo", "Rome"
        };
    }

    public List<string> GetCities()     // Here implement the contract 'List<string> GetCities()'
    {
        return _cities;
    }
}
