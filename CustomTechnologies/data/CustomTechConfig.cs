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
    public static ConfigEntry<bool> DebugMode;
    
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
        
        DebugMode = configFile.Bind("Debug", "Debug", 
            false, new ConfigDescription(
                "enable debug patches and logging, DO NOT USE IN NORMAL PLAY"));
        
    }
}