using System;
using System.Collections.Generic;
using ProcessorTycoon.CompanySystem;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.MarketSystem;

namespace CustomTechnologies.data.companies;

public class CustomNamePattern: CpuNameTemplate.Pattern
{
    private Dictionary<Segment, string> segmentSuffixes;
    
    public CustomNamePattern(List<SegmentSuffixes> segmentSuffixes)
    {
        this.segmentSuffixes = new Dictionary<Segment, string>();
        foreach (var segment in Enum.GetValues(typeof(Segment)))
        {
            var suffix = segmentSuffixes.Find(s => s.Segment == (Segment) segment);
            if (suffix != null)
            {
                this.segmentSuffixes.Add((Segment) segment, suffix.Suffix);
            }
            else
            {
                this.segmentSuffixes.Add((Segment) segment, "");
            }
        }
    }

    public new string GenerateName(Cpu cpu, ICompanyOwner owner, Segment mainMarket, int currentYear)
    {
        currentCpu = cpu;
        this.owner = owner;
        this.mainMarket = mainMarket;
        this.currentYear = currentYear;
        generationYear = GenerationYear <= 0 ? IntroductionYear : GenerationYear;
        switch (nameType)
        {
            case NameType.Generation:
                return GenerateGenerationBasedName();
            case NameType.GenerationRomanModel:
                return GenerateGenerationBasedName(true);
            default:
                return base.GenerateName(cpu, owner, mainMarket, currentYear);
        }
    }

    private string GenerateGenerationBasedName(bool roman = false)
    {
        int generation = GetGenerations(currentCpu);
        if (IsMainMarket(this.currentCpu))
        {
            generation++;
        }
        string generationString = generation.ToString();

        if (roman)
        {
            generationString = GetRomanNumeral(generation);
        }

        return $"{baseName}{generationString}{segmentSuffixes[currentCpu.Strategy.TargetMarket]}";
    }
}