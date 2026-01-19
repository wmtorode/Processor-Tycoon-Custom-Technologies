using System.Collections.Generic;
using BepInEx.Configuration;

namespace CustomTechnologies.data;

public class CustomTechConfig
{
    public static ConfigEntry<string> PackagingTechDir;
    public static ConfigEntry<string> ProcessNodeTechDir;
    
    public static void InitConfig(ConfigFile configFile)
    {
        PackagingTechDir = configFile.Bind("Tech", "Packages", 
            "CustomTechnologies/Packages", new ConfigDescription(
            "Directories where custom package types can be added, ; separated"));
        
        ProcessNodeTechDir = configFile.Bind("Tech", "ProcessNodes", 
            "CustomTechnologies/ProcessNodes", new ConfigDescription(
                "Directories where custom process nodes can be added, ; separated"));
        
    }
}