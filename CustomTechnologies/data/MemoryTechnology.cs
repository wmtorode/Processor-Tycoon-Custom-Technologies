using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class MemoryTechnology: ICustomTech
{
    
    public ResearchTechnology Research;
    public String Name;
    public float IpsThreshold;
    public float CacheIps;
    public float CacheIpcBoost;
    public float UnitCostPerCache;
    public float ProjectCost;
    public int ProjectTime;
    
    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    
    [JsonIgnore]
    public string TechId => Name;
    [JsonIgnore]
    public TechType Type => TechType.Memory;
}