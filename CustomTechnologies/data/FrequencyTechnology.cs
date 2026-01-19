using System;

namespace CustomTechnologies.data;

public class FrequencyTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public float Frequency;
    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.Frequency;
}