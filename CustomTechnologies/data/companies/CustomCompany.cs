using Newtonsoft.Json;

namespace CustomTechnologies.data.companies;

public class CustomCompany
{
    public string BaseCompanyName;
    public string CompanyName;
    public string FullName;
    public float InitialCash;
    public int StartingTechYear;
    public int InitialFactoryCapacity;
    public int FoundingYear;
    public int FoundingMonth;
    public int SpawnYear;
    public int SpawnMonth;

    [JsonIgnore]
    public bool hasSpawned = false;
}