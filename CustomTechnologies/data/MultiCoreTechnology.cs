using System;

namespace CustomTechnologies.data;

public class MultiCoreTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public bool EnablesSmt;
    public int CoreCount;
    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.Multicore;
}