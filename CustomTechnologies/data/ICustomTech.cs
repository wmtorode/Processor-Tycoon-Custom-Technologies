using System;

namespace CustomTechnologies.data;

public interface ICustomTech
{
    ResearchTechnology ResearchTechnology { get; }
    String TechId { get; }
    
    TechType Type { get; }
}