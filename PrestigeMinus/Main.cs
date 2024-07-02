﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityModManagerNet;
using Kingmaker.Blueprints.JsonSystem;
using BlueprintCore.Utils;

namespace PrestigeMinus;

#if DEBUG
[EnableReloading]
#endif
public static class Main {
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger log;
    public static readonly LogWrapper Logger = LogWrapper.Get("PrestigeMinus");

    public static bool Load(UnityModManager.ModEntry modEntry) {
        log = modEntry.Logger;
#if DEBUG
        modEntry.OnUnload = OnUnload;
#endif
        modEntry.OnGUI = OnGUI;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        return true;
    }

    public static void OnGUI(UnityModManager.ModEntry modEntry) {

    }

#if DEBUG
    public static bool OnUnload(UnityModManager.ModEntry modEntry) {
        HarmonyInstance.UnpatchAll(modEntry.Info.Id);
        return true;
    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    static class BlueprintsCaches_Patch
    {
        private static bool Initialized = false;
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        static void Init()
        {
            try
            {
                if (Initialized)
                {
                    Logger.Info("Prestige Minus started already");
                    return;
                }
                Logger.Info("Prestige Minus started");
                Initialized = true;
                MinusAbility.Configure();
                MinusAbility.Configure2();
                Above20.Patch();
            }
            catch (Exception e) { Logger.Error("Prestige Minus failed", e); }

        }
    }
#endif
}
