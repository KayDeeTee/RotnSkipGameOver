using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RhythmRift;
using Shared;

namespace SkipGameOver;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class SkipGameOverPlugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        var gameVersion = BuildInfoHelper.Instance.BuildId.Split('-')[0];

        Logger.LogInfo(gameVersion);

        SkipGameOver.Config.Initialize(Config);
        if (gameVersion != "1.7.0" && gameVersion != "1.7.1" && !SkipGameOver.Config.General.DisableVersionCheck.Value)
        {
            Logger.LogInfo("Invalid game version, ask for an update or disable version check in config");
        }
        else
        {
            Harmony.CreateAndPatchAll(typeof(SkipGameOverPlugin));
        }          
    }

    [HarmonyPatch(typeof(RRStageController), "HandlePlayerDefeat")]
    [HarmonyPrefix]
    public static bool HandlePlayerDefeat(RRStageController __instance)
    {
        if (!SkipGameOver.Config.General.AutoRestart.Value) return true;
        if (__instance.CanQuickRetry)
        {
            __instance.RetryStage(false, true);
            return false;
        }
        return true;
        
    }
}
