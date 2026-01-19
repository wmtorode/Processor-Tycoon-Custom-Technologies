using System;

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
    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.WaferSize;
}