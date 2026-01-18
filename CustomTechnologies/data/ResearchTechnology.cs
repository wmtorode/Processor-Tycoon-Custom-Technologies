using System;
using System.Collections.Generic;

namespace CustomTechnologies.data;

public class ResearchTechnology
{
    public String BaseId;
    public int Year = 0;
    public float ResearchDays = 0f;
    public float MonthlyCost = 0f;
    public float TreeYOffset = 0f;
    public List<String> DependencyIds = new List<string>();
}