using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using CustomTechnologies.data;
using CustomTechnologies.features;
using HarmonyLib;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.ResearchSystem;


namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(ResearchDataProvider), "Awake")]
class ResearchDataProvider_Awake
{
    public static void Postfix(ResearchDataProvider __instance)
    {
        TechnologiesInjector.Instance.InjectTechnologies(__instance);

        if (CustomTechConfig.DumpTech.Value)
        {
            var dumpDirectory = Path.Combine(Paths.PluginPath, CustomTechConfig.TechDumpDir.Value);
            TechnologiesInjector.Instance.DumpTechnologies(__instance, dumpDirectory);
        }
        
        TechnologiesInjector.Instance.ApplyTechPatches(__instance);
        
    }
}

[HarmonyPatch(typeof(ResearchDataProvider), "GetTechnologiesOfType", new Type[] { typeof(TechnologyType) })]
class ResearchDataProvider_GetTechnologiesOfTypeWafer
{
    public static void Postfix(ResearchDataProvider __instance, TechnologyType technologyType, ref List<Technology> __result)
    {
        // wafers need to be sorted by size for upgrades to apply correctly
        if (technologyType == TechnologyType.WaferSize)
        {
            __result = __result.OrderBy(t => t.gameObject.GetComponent<WaferSize>().Value).ToList();
        }
        
    }
}

[HarmonyPatch]
class ResearchDataProvider_GetTechnologiesOfType
{
    
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(ResearchDataProvider), nameof(ResearchDataProvider.GetTechnologiesOfType))
            .MakeGenericMethod(typeof(WaferSize));
    }
    
    public static void Postfix(ResearchDataProvider __instance, ref List<WaferSize> __result)
    {
        // wafers need to be sorted by size for upgrades to apply correctly
        __result = __result.OrderBy(t => t.Value).ToList();
        
    }
}