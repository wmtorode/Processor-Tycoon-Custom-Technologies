using System.IO;
using BepInEx;
using CustomTechnologies.data;
using CustomTechnologies.features;
using HarmonyLib;
using ProcessorTycoon.ResearchSystem;


namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(Technology), "Awake")]

class Technology_Awake
{
    public static void Postfix(Technology __instance)
    {
        if (CustomTechConfig.DumpTech.Value)
        {
            var dumpDirectory = Path.Combine(Paths.PluginPath, CustomTechConfig.TechDumpDir.Value);
            TechnologiesInjector.Instance.DumpTechnology(__instance, dumpDirectory);
        }
    }
}