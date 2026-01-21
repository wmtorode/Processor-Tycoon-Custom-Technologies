using System;
using CustomTechnologies.data;
using HarmonyLib;
using ProcessorTycoon.AISystem;
using ProcessorTycoon.Hardware;

namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(AIBehaviourCreation), "FindBestCpuDesign")]

class AIBehaviourCreation_FindBestCpuDesign
{
    
    public static bool Prepare()
    {
        return CustomTechConfig.DebugMode.Value;
    }

    private static void LogDesign(CpuDesign design)
    {
        CustomTechnologiesPlugin.Logger.LogInfo($"Found design:");
        CustomTechnologiesPlugin.Logger.LogInfo($"Node: {design.ProcessNode?.name}");
        CustomTechnologiesPlugin.Logger.LogInfo($"Memory: {design.Memory?.name}");
        CustomTechnologiesPlugin.Logger.LogInfo($"Package: {design.Package?.name}");
        CustomTechnologiesPlugin.Logger.LogInfo($"Cost: {design.ProjectCost}");
        CustomTechnologiesPlugin.Logger.LogInfo($"Time: {design.ProjectTime}");
        CustomTechnologiesPlugin.Logger.LogInfo($"Market: {design.TargetMarket}");
    }
    
    public static bool Prefix(AIBehaviourCreation __instance)
    {
        CpuDesign[] cpuDesignArray = __instance.SimulateCpus(__instance.simulationAmount);
        if (cpuDesignArray == null || cpuDesignArray.Length == 0)
        {
            CustomTechnologiesPlugin.Logger.LogInfo($"FindBestCpuDesign for: {__instance.company.Name} had no entries");
            return false;
        }
        CustomTechnologiesPlugin.Logger.LogInfo($"FindBestCpuDesign for: {__instance.company.Name} had {cpuDesignArray.Length} entries");
        LogDesign(cpuDesignArray[0]);
        CpuDesign usedDesign = cpuDesignArray[0];
        __instance.bestCpus.Add(usedDesign);
        __instance.UpdateDesignCache(usedDesign);
        return false;
    }
}