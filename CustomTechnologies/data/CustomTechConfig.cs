using BepInEx.Configuration;

namespace CustomTechnologies.data;

public class CustomTechConfig
{
    public static ConfigEntry<string> PackagingTechDir;
    
    public static void InitConfig(ConfigFile configFile)
    {
        PackagingTechDir = configFile.Bind("Tech", "Packages", "CustomTechnologies/Packages", new ConfigDescription(
            "Directory where custom package types can be added"));
        
    }
}