using System.IO;
using BepInEx;
using CustomTechnologies.data;
using CustomTechnologies.features;
using HarmonyLib;
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