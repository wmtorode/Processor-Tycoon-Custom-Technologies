using System.Collections.Generic;
using CustomTechnologies.data;
using HarmonyLib;
using ProcessorTycoon;
using ProcessorTycoon.AISystem;
using ProcessorTycoon.Bank;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.ProjectSystem;
using ProcessorTycoon.TimeSystem;

namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(AIBehaviourResearchProject), "UpdateRating")]

class AIBehaviourResearchProject_UpdateRating
{
    

    
    public static bool Prefix(AIBehaviourResearchProject __instance, ref float __result)
    {
        float num1 = 0.0f;
        int num2 = 6;
        List<Project> researchProjects = __instance.company.ResearchProjects;
        double num3 =  __instance.company.MoneyAmount + CentralBank.Instance.DebtLimit;
        if (num3 > 5000000000.0)
            num1 = 1f;
        if (num3 > 24999999488.0)
            num2 = 10;
        if (num3 > 49999998976.0)
            num2 = 15;
        if (num3 > 99999997952.0)
            num2 = 20;
        if (num3 > 199999995904.0)
            num2 = 40;
        if (num3 > 399999991808.0)
            num2 = 60;
        if (num3 > 599999971328.0)
            num2 = 100;
        if (num3 > 1500000026624.0)
            num2 = 150;
        if (num3 > 1999999991808.0)
            num2 = 200;
        if (num3 > 3000000053248.0)
            num2 = 300;
        if (num3 > 4999999913984.0)
            num2 = 500;
        int difficultyLevel = Player.Instance.DifficultyLevel;
        if (difficultyLevel == 1)
            num2 /= 2;
        if (difficultyLevel == 0)
            num2 /= 3;
        if (researchProjects.Count >= num2)
            num1 = 0.0f;
        if (DateController.Instance.CurrentDate.Year < CustomTechConfig.YearAiCanResearchProjects.Value)
            num1 = 0.0f;
        __result = num1;
        return false;
    }
}