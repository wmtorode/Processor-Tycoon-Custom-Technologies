using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProcessorTycoon.MarketSystem;

namespace CustomTechnologies.data.companies;

public class SegmentSuffixes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public Segment Segment;
    public string Suffix;
}