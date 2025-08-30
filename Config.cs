using BepInEx.Configuration;

namespace SkipGameOver;


//straight up yonked this from shadows reanimated get owned
public static class Config
{
    public class ConfigGroup(ConfigFile config, string group)
    {
        public ConfigEntry<T> Bind<T>(string key, T defaultValue, string description)
        {
            return config.Bind(group, key, defaultValue, description);
        }

        public ConfigEntry<T> Bind<T>(string key, T defaultValue, ConfigDescription description = null)
        {
            return config.Bind(group, key, defaultValue, description);
        }
    }

    public static class General
    {
        public static ConfigEntry<bool> AutoRestart;
        public static ConfigEntry<bool> DisableVersionCheck;

        public static void Initialize(ConfigGroup config)
        {
            AutoRestart = config.Bind("Auto-Restart", true, "Enable restarting instead of going to game over screen.");
            DisableVersionCheck = config.Bind("Disable Version Check", false, "If you think it'll run on current patch without an update.");
        }
    }


    public static void Initialize(ConfigFile config)
    {
        General.Initialize(new(config, "General"));
    }
}