using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BepInEx.Logging;
using CustomTechnologies.data;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.ResearchSystem;
using UnityEngine;


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
    
    private List<ICustomTech> customTechnologies;
    
    private void Initialize()
    {
        customTechnologies = new List<ICustomTech>();
        Logger = CustomTechnologiesPlugin.Logger;
        HasInitialized = true;
    }

    public void LoadCustomTechnologies<T>(String directory) where T : ICustomTech
    {
        Logger.LogInfo($"Loading Custom Packages from {directory}");
        
        if (!Directory.Exists(directory))
            return;

        foreach (var customTechFile in Directory.GetFiles(directory, "*.json"))
        {
            try
            {
                var tech = JsonConvert.DeserializeObject<T>(File.ReadAllText(customTechFile));  
                customTechnologies.Add(tech);
                Logger.LogInfo($"Loaded Custom Tech {tech.TechId} ({tech.Type})");
            } 
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Custom Tech from {customTechFile}: {ex.Message}");
            }
        }
    }
    

    public void InjectTechnologies(ResearchDataProvider  researchDataProvider)
    {
        foreach (var customTech in customTechnologies)
        {

            bool exists = researchDataProvider.allTechnologies.Exists(t => t.name == customTech.TechId);
            if (exists)
            {
                Logger.LogWarning($"Technology {customTech.TechId} already exists, skipping injection");
                continue;
            }
            
            var baseTech = FindBaseTech(researchDataProvider.allTechnologies, customTech.ResearchTechnology.BaseId);

            if (baseTech == null)
            {
                Logger.LogError($"Unable to find BaseTech {customTech.ResearchTechnology.BaseId} for Tech {customTech.TechId}");
                continue;
            }

            var gameObj = GameObject.Instantiate(baseTech.gameObject);
            gameObj.name = customTech.TechId;
            
            var technology = gameObj.GetComponent<Technology>();
            technology.Year = customTech.ResearchTechnology.Year;
            technology.BaseTime = customTech.ResearchTechnology.ResearchDays;
            technology.Cost = customTech.ResearchTechnology.MonthlyCost;
            technology.Offset = customTech.ResearchTechnology.TreeYOffset;
            technology.ID = researchDataProvider.allTechnologies.Count;
            
            technology.Dependencies.Clear();
            
            switch (customTech.Type)
            {
                case TechType.Package:
                    InjectPackages(gameObj.GetComponent<Package>(), customTech as PackageTechnology);
                    break;
                case  TechType.ProcessNode:
                    InjectProcessNodes(gameObj.GetComponent<ProcessNode>(), customTech as ProcessNodeTechnology);
                    break;
            }
            
            researchDataProvider.allTechnologies.Add(technology);
            
            Logger.LogInfo($"Injected Custom Technology: {customTech.TechId} type: {customTech.Type}");
            
        }

        foreach (var technology in customTechnologies)
        {
            var tech = FindBaseTech(researchDataProvider.allTechnologies, technology.TechId);
            foreach (var dependencyId in technology.ResearchTechnology.DependencyIds)
                tech.Dependencies.Add(FindBaseTech(researchDataProvider.allTechnologies, dependencyId));
            
        }
        
    }
    
    private Technology FindBaseTech(List<Technology> technologies, String baseId)
    {
        foreach (var technology in technologies)
        {
            if (technology.name == baseId)
            {
                return technology;
            }
        }
        return null;
    }
    

    private void InjectPackages(Package package, PackageTechnology packageTechnology)
    {
        package.Name = packageTechnology.Name;
        package.baseName = packageTechnology.BaseName;
        package.MinSize = packageTechnology.MinSize;
        package.MaxSize = packageTechnology.MaxSize;
        package.MinPinCount = packageTechnology.MinPinCount;
        package.MaxPinCount = packageTechnology.MaxPinCount;
        package.MinCacheKB = packageTechnology.MinCacheKB;
        package.MaxCacheKB = packageTechnology.MaxCacheKB;
        package.ConsumptionMultiplier = packageTechnology.ConsumptionMultiplier;
        package.SafeTemperature = packageTechnology.SafeTemperature;
        package.ThermalEfficiency = packageTechnology.ThermalEfficiency;
        package.BaseUnitCost = packageTechnology.BaseUnitCost;
        package.ProjectCost = packageTechnology.ProjectCost;
        package.ProjectTime = packageTechnology.ProjectTime;
        package.SupportsMultipleCores = packageTechnology.SupportsMultipleCores;
    }

    private void InjectProcessNodes(ProcessNode processNode, ProcessNodeTechnology processNodeTechnology)
    {
        processNode.Name = processNodeTechnology.Name;
        processNode.minimumYieldRate = processNodeTechnology.MinimumYieldRate;
        processNode.maximumYieldRate = processNodeTechnology.MaximumYieldRate;
        processNode.CacheCostReduction = processNodeTechnology.CacheCostReduction;
        processNode.LearningRequirement = processNodeTechnology.LearningRequirement;
        processNode.TransistorDensity = processNodeTechnology.TransistorDensity;
        processNode.PowerConsumptionReduction = processNodeTechnology.PowerConsumptionReduction;
        processNode.MulticorePenaltyReduction = processNodeTechnology.MulticorePenaltyReduction;
        processNode.ProjectCost = processNodeTechnology.ProjectCost;
        processNode.ProjectTime = processNodeTechnology.ProjectTime;
        processNode.FailureRateOffset = processNodeTechnology.FailureRateOffset;
    }
    
    
}