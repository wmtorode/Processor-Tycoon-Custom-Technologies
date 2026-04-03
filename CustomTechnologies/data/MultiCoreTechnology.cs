using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class MultiCoreTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public bool EnablesSmt;
    public int CoreCount;
    
    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    [JsonIgnore]
    public string TechId => Name;
    [JsonIgnore]
    public TechType Type => TechType.Multicore;
}