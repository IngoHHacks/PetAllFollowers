using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Lamb.UI.FollowerInteractionWheel;
using System.Collections.Generic;
using System.IO;

namespace PetAllFollowers
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "IngoH.cotl.PetAllFollowers";
        public const string PluginName = "PetAllFollowers";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }

        [HarmonyPatch(typeof(FollowerCommandGroups), "DefaultCommands")]
        [HarmonyPostfix]
        public static void FollowerCommandGroups_DefaultCommands(Follower follower, ref List<CommandItem> __result)
        {
            if (!WorshipperData.Instance.Characters[follower.Brain.Info.SkinCharacter].Title.Contains("Dog")) {
                __result.Add(FollowerCommandItems.PetDog());
            }
        }

        [HarmonyPatch(typeof(CommandItem), "GetTitle")]
        [HarmonyPostfix]
        public static void CommandItem_GetTitle(ref string __result)
        {
            if (__result == "Pet Dog")
            {
                __result = "Pet";
            }
        }

        [HarmonyPatch(typeof(CommandItem), "GetDescription")]
        [HarmonyPostfix]
        public static void CommandItem_GetDescription(ref string __result)
        {
            if (__result == "Who's a good dog? Yes, you are!")
            {
                __result = "Who's a good Follower? Yes, you are!";
            }
        }
    }
}