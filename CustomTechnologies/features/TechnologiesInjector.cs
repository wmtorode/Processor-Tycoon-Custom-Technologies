using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BepInEx.Logging;
using CustomTechnologies.data;
using ProcessorTycoon.Hardware;

namespace CustomTechnologies.features;

public class TechnologiesInjector
{
    
    public static TechnologiesInjector Instance
    {
        get
        {
            if (_instance == null) _instance = new TechnologiesInjector();
            if (!_instance.HasInitialized) _instance.Initialize();
            return _instance;
        }
    }
    
    private static TechnologiesInjector _instance;
    private bool HasInitialized = false;
    private ManualLogSource Logger;
    
    private List<PackageTechnology> packageTechnologies;
    
    private void Initialize()
    {
        packageTechnologies = new List<PackageTechnology>();
        Logger = CustomTechnologiesPlugin.Logger;
        HasInitialized = true;

    }

    public void LoadCustomPackages(String packageDirectory)
    {
        Logger.LogInfo($"Loading Custom Packages from {packageDirectory}");

        foreach (var packageFile in Directory.GetFiles(packageDirectory, "*.json"))
        {
            try
            {
              var package = JsonConvert.DeserializeObject<PackageTechnology>(File.ReadAllText(packageFile));  
              packageTechnologies.Add(package);
              Logger.LogInfo($"Loaded Custom Package {package.Name}");
            } 
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Custom Package from {packageFile}: {ex.Message}");
                Logger.LogError($"Failed to load Custom Package Inner: {ex.InnerException?.Message}");
            }
        }
        
    }
    
    
}