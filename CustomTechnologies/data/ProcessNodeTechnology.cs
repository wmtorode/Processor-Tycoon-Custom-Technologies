using System;

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

    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.ProcessNode;
}