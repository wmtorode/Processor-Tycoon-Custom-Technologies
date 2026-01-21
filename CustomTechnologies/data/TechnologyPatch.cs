using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CustomTechnologies.data;

public class TechnologyPatch
{
    public String TechId;
    public int? Year = null;
    public int? TreeYOffset = null;
    [CanBeNull] 
    public List<String> DependencyIds = null;
}