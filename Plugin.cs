using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Il2CppSystem;

namespace publicPractice
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static ConfigEntry<string> lobbyName;
        public static Plugin Instance;

        public static readonly Dictionary<string, string> lobbyMetadata = new Dictionary<string, string>(){
            {"Modes", "11111111111111111"},
            {"AllMaps", "False"},
            {"Maps", "111111111111111111111111111111111111111111111111111111111111111"},
            {"LobbyKeyId", "LobbyValId"},
            {"Voice Chat", "True"},
            {"LobbyState", "Lobby"},
            //{"Version", "1.362"}
        };
        public override void Load()
        {
            Instance = this;
            Harmony.CreateAndPatchAll(typeof(Plugin));
            Harmony.CreateAndPatchAll(typeof(patch2));

            lobbyName = Config.Bind<string>("Public Practice","Lobby Name", "Public Practice","The name of your practice lobby. this updates whenever you press F10.");

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
        [HarmonyPatch(typeof(GameUI),"Update")]
        [HarmonyPrefix]
        public static void Prefix(GameUI __instance){
            if (Input.GetKeyDown(KeyCode.F10)){
                Instance.Config.Reload();
                SteamworksNative.CSteamID lobbyID = SteamManager.Instance.currentLobby;
                SteamworksNative.SteamMatchmaking.SetLobbyType(lobbyID, SteamworksNative.ELobbyType.k_ELobbyTypePublic);
                SteamworksNative.SteamMatchmaking.SetLobbyJoinable(lobbyID, true);
                SteamworksNative.SteamMatchmaking.SetLobbyData(lobbyID, "LobbyName", lobbyName.Value);
                foreach (KeyValuePair<string,string> pair in lobbyMetadata){
                    SteamworksNative.SteamMatchmaking.SetLobbyData(lobbyID, pair.Key, pair.Value);
                }
                SteamworksNative.SteamMatchmaking.SetLobbyType(lobbyID, SteamworksNative.ELobbyType.k_ELobbyTypePublic);
            }
            if (Input.GetKeyDown(KeyCode.F9)){
                SteamworksNative.CSteamID lobbyID = GameObject.Find("/Managers/SteamManager").GetComponent<MonoBehaviourPublicObInUIgaStCSBoStcuCSUnique>().currentLobby;
                SteamworksNative.SteamMatchmaking.SetLobbyType(lobbyID, SteamworksNative.ELobbyType.k_ELobbyTypeInvisible);
            }
        }
        [HarmonyPatch]
        [HarmonyPatch(typeof(MonoBehaviourPublicGataInefObInUnique), "Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicCSDi2UIInstObUIloDiUnique), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicVesnUnique), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicObjomaOblogaTMObseprUnique), "Method_Public_Void_PDM_2")]
        [HarmonyPatch(typeof(MonoBehaviourPublicTeplUnique), "Method_Private_Void_PDM_32")]
        static class patch2{
            [HarmonyPrefix]
            public static bool Detect(System.Reflection.MethodBase __originalMethod)
            {
                return false;
            }
        }
    }
}
