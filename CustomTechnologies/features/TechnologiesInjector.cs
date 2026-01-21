using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using BepInEx.Logging;
using CustomTechnologies.data;
using ProcessorTycoon.Hardware;
using ProcessorTycoon.Hardware.Math;
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
    private List<TechnologyPatch> patches;
    
    private void Initialize()
    {
        customTechnologies = new List<ICustomTech>();
        patches = new List<TechnologyPatch>();
        Logger = CustomTechnologiesPlugin.Logger;
        HasInitialized = true;
    }

    public void LoadCustomTechnologies<T>(String directory) where T : ICustomTech
    {
        Logger.LogInfo($"Loading Custom Technologies from {directory}");
        
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

    public void LoadTechPatches(String directory)
    {
        Logger.LogInfo($"Loading Tech Patches from {directory}");
        
        if (!Directory.Exists(directory))
            return;

        foreach (var patchFile in Directory.GetFiles(directory, "*.json"))
        {
            try
            {
                var tech = JsonConvert.DeserializeObject<TechnologyPatch>(File.ReadAllText(patchFile));  
                patches.Add(tech);
                Logger.LogInfo($"Loaded Patch for tech {tech.TechId}");
            } 
            catch (Exception ex)
            {
                Logger.LogError($"Failed to load Tech Patch from {patchFile}: {ex.Message}");
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
                case TechType.Memory:
                    InjectMemory(gameObj.GetComponent<Memory>(), customTech as MemoryTechnology);
                    break;
                case TechType.Frequency:
                    InjectFrequency(gameObj.GetComponent<Frequency>(), customTech as FrequencyTechnology);
                    break;
                case TechType.Cache:
                    InjectCache(gameObj.GetComponent<CacheSize>(), customTech as CacheTechnology);
                    break;
                case TechType.WaferSize:
                    InjectWafer(gameObj.GetComponent<WaferSize>(), customTech as WaferTechnology);
                    break;
                case TechType.Multicore:
                    InjectCores(gameObj.GetComponent<Multicore>(), customTech as MultiCoreTechnology);
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
        
        ReEnumerateTechnologies(researchDataProvider.allTechnologies);
        researchDataProvider.allTechnologies =
            researchDataProvider.allTechnologies.OrderBy(tech => tech.ID).ToList();
        
        Logger.LogInfo("Technologies Injected, new ID Ordering:");
        foreach (var tech in researchDataProvider.allTechnologies)
        {
            Logger.LogInfo($"{tech.ID}: {tech.name}");
        }
        UpdateHardwareMath(researchDataProvider.allTechnologies);

    }

    public void ApplyTechPatches(ResearchDataProvider researchDataProvider)
    {
        Logger.LogInfo($"Applying Patches");

        foreach (var patch in patches)
        {
            var tech = FindBaseTech(researchDataProvider.allTechnologies, patch.TechId);
            if (tech == null)
            {
                Logger.LogWarning($"Tech {patch.TechId} not found, skipping patch");
                continue;
            }
            
            Logger.LogInfo($"Patching {patch.TechId}");

            if (patch.Year != null)
            {
                Logger.LogInfo($"Patching Year from {tech.Year} to {patch.Year}");
                tech.Year = patch.Year.Value;
            }
            
            if (patch.TreeYOffset != null)
            {
                Logger.LogInfo($"Patching TreeYOffset from {tech.Offset} to {patch.TreeYOffset}");
                tech.Offset = patch.TreeYOffset.Value;
            }
            
            if (patch.DependencyIds != null)
            {
                Logger.LogInfo($"Patching Dependencies");
                tech.Dependencies.Clear();
                foreach (var dependencyId in patch.DependencyIds)
                {
                    tech.Dependencies.Add(FindBaseTech(researchDataProvider.allTechnologies, dependencyId));
                    Logger.LogInfo($"Added Dependency: {dependencyId}");
                }
            }
        }
    }

    public void DumpTechnologies(ResearchDataProvider researchDataProvider, String directory)
    {
        
        Logger.LogInfo($"Dumping Technologies to {directory}");
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        
        foreach (var tech in researchDataProvider.allTechnologies)
        {
            var techFile = Path.Combine(directory, $"{tech.name.Replace(" ", "")}.json");
            var techPatch = new TechnologyPatch();
            techPatch.TechId = tech.name;
            techPatch.Year = tech.Year;
            techPatch.TreeYOffset = tech.Offset;
            techPatch.DependencyIds = tech.Dependencies.Select(d => d.name).ToList();
            File.WriteAllText(techFile, JsonConvert.SerializeObject(techPatch, Formatting.Indented));
        }
        Logger.LogInfo("Technologies Dumped");
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
    
    private void InjectMemory(Memory memory, MemoryTechnology memoryTechnology)
    {
        memory.Name = memoryTechnology.Name;
        memory.IpsThreshold = memoryTechnology.IpsThreshold;
        memory.CacheIps = memoryTechnology.CacheIps;
        memory.CacheIpcBoost = memoryTechnology.CacheIpcBoost;
        memory.UnitCostPerCache = memoryTechnology.UnitCostPerCache;
        memory.ProjectCost = memoryTechnology.ProjectCost;
        memory.ProjectTime = memoryTechnology.ProjectTime;
    }

    private void InjectFrequency(Frequency frequency, FrequencyTechnology frequencyTechnology)
    {
        frequency.Name = frequencyTechnology.Name;
        frequency.Value = frequencyTechnology.Frequency;
    }
    
    private void InjectCache(CacheSize cache, CacheTechnology cacheTechnology)
    {
        cache.Name = cacheTechnology.Name;
        cache.Steps = cacheTechnology.L1Steps4K;
        cache.StepsL2 = cacheTechnology.L2Steps16K;
        cache.StepsL3 = cacheTechnology.L3Steps64K;
    }
    
    private void InjectWafer(WaferSize wafer, WaferTechnology waferTechnology)
    {
        wafer.Name = waferTechnology.Name;
        wafer.NormalName = waferTechnology.NormalName;
        wafer.Value = waferTechnology.WaferSize;
        wafer.ConstructionCostMultiplier = waferTechnology.ConstructionCostMultiplier;
        wafer.MaintainanceCostMultiplier = waferTechnology.MaintainanceCostMultiplier;
        wafer.UpgradeCost = waferTechnology.UpgradeCost;
    }

    private void InjectCores(Multicore multicore, MultiCoreTechnology multicoreTechnology)
    {
        multicore.Name = multicoreTechnology.Name;
        multicore.CoreCount = multicoreTechnology.CoreCount;
        multicore.EnablesSmt = multicoreTechnology.EnablesSmt;
    }

    private void ReEnumerateTechnologies(List<Technology> technologies)
    {
        Logger.LogInfo($"ReEnumerating technologies");
        
        Logger.LogInfo($"ReEnumerating Wafers");
        var wafers = technologies.Where(t => t.gameObject.GetComponent<WaferSize>() != null).ToList();
        
        ReEnumerateWafers(wafers);
        
        Logger.LogInfo($"ReEnumerating Packages");
        var packages = technologies.Where(t => t.gameObject.GetComponent<Package>() != null).ToList();
        
        ReEnumerateTechByYear(packages);
        
        Logger.LogInfo($"ReEnumerating Cores");
        var cores = technologies.Where(t => t.gameObject.GetComponent<Multicore>() != null).ToList();
        
        ReEnumerateTechByYear(cores);
        
        Logger.LogInfo($"ReEnumerating Lithography Nodes");
        var nodes = technologies.Where(t => t.gameObject.GetComponent<ProcessNode>() != null).ToList();
        
        ReEnumerateTechByYear(nodes);
        
        Logger.LogInfo($"ReEnumerating Memory");
        var memory = technologies.Where(t => t.gameObject.GetComponent<Memory>() != null).ToList();
        
        ReEnumerateTechByYear(memory);
        
        Logger.LogInfo($"ReEnumerating Caches");
        var caches = technologies.Where(t => t.gameObject.GetComponent<CacheSize>() != null).ToList();
        
        ReEnumerateTechByYear(caches);
        
    }

    private void ReEnumerateWafers(List<Technology> technologies)
    {
        // wafers need to be enumerated in order of their size, otherwise the game doesn't upgrade them correctly
        Logger.LogInfo($"Wafers enumerated: {technologies.Count}");
        var waferIds = technologies.Select(t => t.ID).ToList();
        var sortedWafers = technologies.OrderBy(t => t.gameObject.GetComponent<WaferSize>().Value).ToList();
        var index = 0;
        foreach (var wafer in sortedWafers)
        {
            Logger.LogInfo($"Wafer: {wafer.name}, Old ID: {wafer.ID}, New ID: {waferIds[index]}");
            wafer.ID = waferIds[index];
            index++;
        }
    }

    private void ReEnumerateTechByYear(List<Technology> technologies)
    {
        Logger.LogInfo($"Techs enumerated: {technologies.Count}");
        var ids = technologies.Select(t => t.ID).ToList();
        var sortedTechs = technologies.OrderBy(t => t.Year).ToList();
        var index = 0;
        foreach (var tech in sortedTechs){
            Logger.LogInfo($"Tech: {tech.name}, Old ID: {tech.ID}, New ID: {ids[index]}");
            tech.ID = ids[index];
            index++;
        }
    }


    private void UpdateHardwareMath(List<Technology> technologies)
    {
        Logger.LogInfo($"Updating HardwareMath");
        var coreCounts = new List<int>() { 1 };
        var coreTech = technologies.Where(t => t.gameObject.GetComponent<Multicore>() != null);
        var cores = coreTech.Select(t => t.gameObject.GetComponent<Multicore>().CoreCount).ToList();
        foreach (var core in cores)
        {
            if (core != 0)
            {
                coreCounts.Add(core);
                Logger.LogInfo($"Added Core Count: {core}");
            }
        }
        coreCounts.Sort();

        HardwareMath.cores = coreCounts.ToArray();
        Logger.LogInfo($"HardwareMath Updated, new core count: {HardwareMath.cores.Length}");
        foreach (var core in HardwareMath.cores)
        {
            Logger.LogInfo($"Core: {core}");
        }
    }


}