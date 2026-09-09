using System.Collections.Generic;
using CustomTechnologies.data;
using ProcessorTycoon.AISystem;
using HarmonyLib;

namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(AIBehaviourResearch), "ChooseANewResearch")]
public static class AIBehaviourResearch_ChooseANewResearch
{
    
    public static bool Prepare()
    {
        return CustomTechConfig.DebugMode.Value;
    }
    
    public static void Postfix(AIBehaviourResearch __instance)
    {
        var currentReseach = __instance.company.ResearchSector.teamOne.CurrentTechnologyID;
        if (currentReseach == null)
        {
            currentReseach = "None";
        }
        CustomTechnologiesPlugin.Logger.LogInfo($"Company: {__instance.company.Name}, selected New Research: {currentReseach}");
    }
}
