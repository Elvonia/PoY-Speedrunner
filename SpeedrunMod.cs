using HarmonyLib;
using MelonLoader;
using PoY_Speedrunning;
using System;
using UnityEngine;

[assembly: MelonInfo(typeof(SpeedrunMod), "Speedrunner", "1.0", "Kalico")]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace PoY_Speedrunning
{
    public class SpeedrunMod : MelonMod
    {
        private string listHeader = "Active Mods:\n";
        private string listOfMods = "";
        private static bool showModList = false;

        public override void OnInitializeMelon()
        {
            foreach (MelonMod mod in RegisteredMelons)
            {
                listOfMods = listOfMods + mod.Info.Name + " by " + mod.Info.Author + "\n";
            }
        }

        public override void OnGUI()
        {
            if (showModList)
            {
                DrawModList();
            }
        }

        private void DrawModList()
        {
            GUIStyle headerStyle = new GUIStyle();
            headerStyle.alignment = TextAnchor.UpperRight;
            headerStyle.normal.textColor = Color.green;
            headerStyle.fontStyle = FontStyle.Bold;

            GUI.Label(new Rect(Screen.width - 500 - 10, 10, 500, 20), listHeader, headerStyle);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperRight;
            style.normal.textColor = Color.red;

            GUI.Label(new Rect(Screen.width - 500 - 10, 30, 500, 80), listOfMods, style);
        }

        [HarmonyPatch(typeof(TimeAttack), "BringUpScore")]
        private static class ModlistEnable
        {
            private static bool Prefix()
            {
                showModList = true;
                return true;
            }
        }

        [HarmonyPatch(typeof(TimeAttack), "AllowClosingTime")]
        private static class ModlistDisable
        {
            private static bool Prefix()
            {
                showModList = false;
                return true;
            }
        }

        // Sets watch to show milliseconds.
        // Has no issue with the way times are saved in registry with or without the modification active
        // Only caveat is when grounded it still shows 00:00:00 instead of 00:00:000 but not worth modifying
        //
        // This change was suggested and will await a determination if it will be a game feature or not

        /*[HarmonyPatch(typeof(TimeAttack), "SetTimeSpan")]
        private static class TimePatch
        {
            private static bool Prefix(float time, ref string __result)
            {
                __result = SetTimeSpan(time);
                return false;
            }

            private static string SetTimeSpan(float time)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
                return string.Format("{0:00}:{1:00}:{2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            }
        }*/
    }
}
