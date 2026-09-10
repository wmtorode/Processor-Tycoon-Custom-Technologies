using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProcessorTycoon.Hardware;

namespace CustomTechnologies.data.companies;

public class CpuNamePattern
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CpuNameTemplate.Pattern.NameType PatternType;
    public string BaseName;
    public int IntroductionYear;
    public int GenerationYear;
    public bool UseCustomGenerator = false;
    public List<SegmentSuffixes> SegmentSuffixes = new();
    
}