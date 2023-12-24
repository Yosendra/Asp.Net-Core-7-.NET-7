namespace ServiceContracts;

public interface ICitiesService
{
    Guid ServiceInstanceId { get; } // notice this
    List<string> GetCities();
}
