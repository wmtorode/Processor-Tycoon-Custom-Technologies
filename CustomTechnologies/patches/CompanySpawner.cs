using System.IO;
using BepInEx;
using CustomTechnologies.data;
using CustomTechnologies.features;
using HarmonyLib;
using ProcessorTycoon.CompanySystem;


namespace CustomTechnologies.patches;

[HarmonyPatch(typeof(CompanySpawner), "SpawnCompanies")]

class CompanySpawner_SpawnCompanies
{
    public static void Postfix(CompanySpawner __instance)
    {
        CompaniesInjector.Instance.InjectCompanies(__instance);
    }
}

[HarmonyPatch(typeof(CompanySpawner.HistoricalCompany), "Spawn")]
class HistoricalCompany_Spawn
{
    public static void Postfix(CompanySpawner.HistoricalCompany __instance)
    {
        CustomTechnologiesPlugin.Logger.LogInfo($"Spawned company: {__instance.companyPrefab.Name}");
    }
}