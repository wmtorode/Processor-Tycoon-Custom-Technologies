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
    }
}