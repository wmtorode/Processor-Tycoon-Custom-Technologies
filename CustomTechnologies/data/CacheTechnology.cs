using System;

namespace CustomTechnologies.data;

public class CacheTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public int L1Steps4K;
    public int L2Steps16K;
    public int L3Steps64K;
    public ResearchTechnology ResearchTechnology => Research;
    public string TechId => Name;
    public TechType Type => TechType.Cache;
}