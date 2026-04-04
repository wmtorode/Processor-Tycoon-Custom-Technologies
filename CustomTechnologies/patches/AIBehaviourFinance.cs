using System;
using System.Collections.Generic;
using System.Linq;
using CustomTechnologies.data;
using HarmonyLib;
using ProcessorTycoon.AISystem;
using ProcessorTycoon.Hardware;

namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(AIBehaviourFinance), "OnTick")]

class AIBehaviourFinance_OnTick
{
    
    public static bool Prepare()
    {
        return true;
        return CustomTechConfig.DebugMode.Value;
    }

    public static void Prefix(AIBehaviourFinance __instance)
    {
        CustomTechnologiesPlugin.Logger.LogInfo($"Fiance OnTick For: {__instance.company.Name}");
    }
}