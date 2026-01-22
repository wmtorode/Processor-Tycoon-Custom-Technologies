using System.Collections.Generic;
using BepInEx.Configuration;

namespace CustomTechnologies.data;

public class CustomTechConfig
{
    public static ConfigEntry<string> PackagingTechDir;
    public static ConfigEntry<string> ProcessNodeTechDir;
    public static ConfigEntry<string> MemoryTechDir;
    public static ConfigEntry<string> FrequencyTechDir;
    public static ConfigEntry<string> WaferSizeTechDir;
    public static ConfigEntry<string> MulticoreTechDir;
    public static ConfigEntry<string> CacheTechDir;
    public static ConfigEntry<string> TechPatchDir;
    public static ConfigEntry<string> TechDumpDir;
    public static ConfigEntry<bool> DebugMode;
    public static ConfigEntry<bool> DumpTech;
    public static ConfigEntry<int> YearAiCanResearchProjects;
    public static ConfigEntry<int> YearPlayerCanResearchProjects;
    
    public static void InitConfig(ConfigFile configFile)
    {
        PackagingTechDir = configFile.Bind("Tech", "Packages", 
            "CustomTechnologies/Packages", new ConfigDescription(
            "Directories where custom package types can be added, ; separated"));
        
        ProcessNodeTechDir = configFile.Bind("Tech", "ProcessNodes", 
            "CustomTechnologies/ProcessNodes", new ConfigDescription(
                "Directories where custom process nodes can be added, ; separated"));
        
        MemoryTechDir = configFile.Bind("Tech", "Memory", 
            "CustomTechnologies/Memory", new ConfigDescription(
                "Directories where custom memory can be added, ; separated"));
        
        FrequencyTechDir = configFile.Bind("Tech", "Frequency", 
            "CustomTechnologies/Frequency", new ConfigDescription(
                "Directories where custom frequency upgrades can be added, ; separated"));
        
        WaferSizeTechDir = configFile.Bind("Tech", "Wafer", 
            "CustomTechnologies/Wafer", new ConfigDescription(
                "Directories where custom wafer sizes can be added, ; separated"));
        
        MulticoreTechDir = configFile.Bind("Tech", "Cores", 
            "CustomTechnologies/Cores", new ConfigDescription(
                "Directories where custom core counts can be added, ; separated"));
        
        CacheTechDir = configFile.Bind("Tech", "Cache", 
            "CustomTechnologies/Cache", new ConfigDescription(
                "Directories where custom cache upgrades can be added, ; separated"));
        
        TechPatchDir = configFile.Bind("Tech", "Patches", 
            "CustomTechnologies/Patches", new ConfigDescription(
                "Directories where existing technologies can be patched, ; separated"));
        
        YearAiCanResearchProjects = configFile.Bind("Research", "YearAiCanResearchProjects", 
            2030, new ConfigDescription(
                "Year AI can use research projects, default 2030"));
        
        YearPlayerCanResearchProjects = configFile.Bind("Research", "YearPlayerCanResearchProjects", 
            2030, new ConfigDescription(
                "Year player can use research projects, default 2030"));
        
        DebugMode = configFile.Bind("Debug", "Debug", 
            false, new ConfigDescription(
                "enable debug patches and logging, DO NOT USE IN NORMAL PLAY"));
        
        DumpTech = configFile.Bind("Debug", "DumpTech", 
            false, new ConfigDescription(
                "enable dumping technologies to the TechDump directory, DO NOT USE IN NORMAL PLAY"));
        
        TechDumpDir = configFile.Bind("Debug", "TechDump", 
            "CustomTechnologies/Dump", new ConfigDescription(
                "Directories where existing technologies will be dumped"));
        
    }
}