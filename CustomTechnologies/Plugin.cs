using System.IO;
using BepInEx;
using BepInEx.Logging;
using CustomTechnologies.data;
using CustomTechnologies.features;

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
        
        var packagePath = Path.Combine(Paths.PluginPath, CustomTechConfig.PackagingTechDir.Value);
        TechnologiesInjector.Instance.LoadCustomPackages(packagePath);
    }
}