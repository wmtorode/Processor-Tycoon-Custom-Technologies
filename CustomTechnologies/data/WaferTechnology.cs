using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class WaferTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public String NormalName;
    public float WaferSize;
    public float ConstructionCostMultiplier;
    public float MaintainanceCostMultiplier;
    public float UpgradeCost;
    
    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    
    [JsonIgnore]
    public string TechId => Name;
    
    [JsonIgnore]
    public TechType Type => TechType.WaferSize;
}