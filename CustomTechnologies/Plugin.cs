using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using CustomTechnologies.data;
using CustomTechnologies.features;
using HarmonyLib;

namespace CustomTechnologies;

[BepInPlugin("ca.jwolf.customTech", "Custom Technologies", "1.0.0")]
public class CustomTechnologiesPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Custom Technologies is loaded!");
        
        // load config
        CustomTechConfig.InitConfig(Config);

        foreach (var techDir in CustomTechConfig.PackagingTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<PackageTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.ProcessNodeTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<ProcessNodeTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.MemoryTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<MemoryTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.FrequencyTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<FrequencyTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.CacheTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<CacheTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.WaferSizeTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<WaferTechnology>(techPath);
        }
        
        foreach (var techDir in CustomTechConfig.MulticoreTechDir.Value.Split(";"))
        {
            var techPath = Path.Combine(Paths.PluginPath, techDir);
            TechnologiesInjector.Instance.LoadCustomTechnologies<MultiCoreTechnology>(techPath);
        }
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "ca.jwolf.customTech");

    }
}