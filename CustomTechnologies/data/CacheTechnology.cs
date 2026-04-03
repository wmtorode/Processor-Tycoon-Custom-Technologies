using System;
using Newtonsoft.Json;

namespace CustomTechnologies.data;

public class CacheTechnology: ICustomTech
{
    public ResearchTechnology Research;
    public String Name;
    public int L1Steps4K;
    public int L2Steps16K;
    public int L3Steps64K;
    
    [JsonIgnore]
    public ResearchTechnology ResearchTechnology => Research;
    [JsonIgnore]
    public string TechId => Name;
    [JsonIgnore]
    public TechType Type => TechType.Cache;
}