using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class PackageTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public String BaseName;
    public float MinSize;
    public float MaxSize;
    public float MinPinCount;
    public float MaxPinCount;
    public int MinCacheKB;
    public int MaxCacheKB;
    public float ConsumptionMultiplier;
    public float SafeTemperature;
    public float ThermalEfficiency;
    public float BaseUnitCost;
    public float ProjectCost;
    public int ProjectTime;
    public bool SupportsMultipleCores;

    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    
    [JsonIgnore]
    public string TechId => Name;
    
    [JsonIgnore]
    public TechType Type => TechType.Package;
}