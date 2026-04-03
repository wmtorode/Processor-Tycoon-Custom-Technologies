using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class ProcessNodeTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public float MinimumYieldRate;
    public float MaximumYieldRate;

    public String Name;
    public float LearningRequirement;
    public float TransistorDensity;
    public float PowerConsumptionReduction;

    public float CacheCostReduction;
    public float MulticorePenaltyReduction;
    public float ProjectCost;
    public int ProjectTime;
    public float FailureRateOffset;

    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    [JsonIgnore]
    public string TechId => Name;
    [JsonIgnore]
    public TechType Type => TechType.ProcessNode;
}