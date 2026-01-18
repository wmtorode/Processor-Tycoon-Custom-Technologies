using System;

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

    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.Package;
}