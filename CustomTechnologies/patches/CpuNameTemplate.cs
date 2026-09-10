using System;
using System.Collections.Generic;
using System.Linq;
using CustomTechnologies.data;
using CustomTechnologies.data.companies;
using HarmonyLib;
using ProcessorTycoon;
using ProcessorTycoon.Bank;
using ProcessorTycoon.CompanySystem;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.MarketSystem;
using ProcessorTycoon.ProjectSystem;
using ProcessorTycoon.ResearchSystem;
using ProcessorTycoon.TimeSystem;
using UnityEngine;

namespace CustomTechnologies.patches;

//Todo: Remove this when pattern.GenerateName is made a virtual
[HarmonyPatch(typeof(CpuNameTemplate), "GenerateName")]
class CpuNameTemplate_UpdateRating
{
    public static bool Prefix(CpuNameTemplate __instance, Cpu cpu, ICompanyOwner owner, Segment mainMarket, ref string __result)
    {
        int year = DateController.Instance.CurrentDate.Year;
        if (__instance.patterns.Count == 0)
        {
            Debug.LogWarning( ("There are no naming schemes for " + owner.Company.Name + "!"));
            __result = "ERROR";
            return false;
        }
        CpuNameTemplate.Pattern pattern1 = __instance.patterns[0];
        foreach (CpuNameTemplate.Pattern pattern2 in __instance.patterns)
        {
            if (year >= pattern2.IntroductionYear)
                pattern1 = pattern2;
        }
        if (year >= 2017 && CompanyHelper.IsEMD(cpu.Company) && cpu.ProcessNode.GetComponent<Technology>().Year < 2015)
            pattern1 = __instance.patterns.Where(pattern => pattern.IntroductionYear == 2011).FirstOrDefault();
        if (year >= 2023 && CompanyHelper.IsInlet(cpu.Company) && cpu.ProcessNode.GetComponent<Technology>().Year < 2023)
            pattern1 = __instance.patterns.Where(pattern => pattern.IntroductionYear == 2009).FirstOrDefault();
        
        
        CustomNamePattern customNamePattern = pattern1 as CustomNamePattern;
        if (customNamePattern != null)
        {
            __result = customNamePattern.GenerateName(cpu, owner, mainMarket, year);
        }
        else
        {
            __result = pattern1.GenerateName(cpu, owner, mainMarket, year);
        }
        return false;
    }
}