using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class FrequencyTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public float Frequency;
    
    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    [JsonIgnore]
    public string TechId => Name;
    [JsonIgnore]
    public TechType Type => TechType.Frequency;
}