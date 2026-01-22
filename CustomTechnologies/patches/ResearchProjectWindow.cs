using System.Collections.Generic;
using CustomTechnologies.data;
using HarmonyLib;
using ProcessorTycoon;
using ProcessorTycoon.AISystem;
using ProcessorTycoon.Bank;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.ProjectSystem;
using ProcessorTycoon.ResearchSystem;
using ProcessorTycoon.TimeSystem;

namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(ResearchProjectWindow), "UpdateLockedState")]

class ResearchProjectWindow_UpdateLockedState
{
    
    public static bool Prefix(ResearchProjectWindow __instance)
    {
        string str = "Research";
        int num = DateController.Instance.CurrentDate.Year < CustomTechConfig.YearPlayerCanResearchProjects.Value ? 1 : 0;
        bool flag1 = false;
        if (Player.Instance.Company.IsFabless && __instance.currentModifier == ResearchProjectWindow.Modifier.Density)
        {
            flag1 = true;
            str = "No Manufacturing Division";
        }
        else if (Player.Instance.Company.IsFoundry && (__instance.currentModifier == ResearchProjectWindow.Modifier.Frequency || __instance.currentModifier == ResearchProjectWindow.Modifier.Consumption))
        {
            flag1 = true;
            str = "No CPU Division";
        }
        bool flag2 = (num | (flag1 ? 1 : 0)) != 0;
        if (num != 0)
            str = $"Locked Until {CustomTechConfig.YearPlayerCanResearchProjects.Value}";
        __instance.researchButton.Interactable = !flag2;
        __instance.researchButton.Text = str;
        return false;
    }
}