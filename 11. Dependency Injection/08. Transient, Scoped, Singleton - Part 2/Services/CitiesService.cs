using ServiceContracts;

namespace Services;

public class CitiesService : ICitiesService
{
    private List<string> _cities;
    private Guid _serviceInstanceId;    // notice this

    public Guid ServiceInstanceId 
    { 
        get => _serviceInstanceId;
    }

    public CitiesService()
    {
        _serviceInstanceId = Guid.NewGuid();    // notice this, pay attention to its value
        _cities = new List<string>()
        {
            "London", "Paris", "New York", "Tokyo", "Rome"
        };
    }

    public List<string> GetCities()
    {
        return _cities;
    }
}
