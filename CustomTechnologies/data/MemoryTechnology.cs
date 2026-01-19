using System;

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
    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.Memory;
}