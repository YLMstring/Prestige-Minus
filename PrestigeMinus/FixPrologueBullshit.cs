using HarmonyLib;
using Kingmaker;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeMinus
{
    [HarmonyPatch(typeof(DialogSpeaker), nameof(DialogSpeaker.NeedsEntity), MethodType.Getter)]
    internal class FixPrologueBullshit
    {
        static void Postfix(ref bool __result, ref DialogSpeaker __instance)
        {
            try
            {
                if (Game.Instance.Player.Chapter > 0)
                {
                    return;
                }
                var ins = __instance;
                if (Game.Instance.Player.RemoteCompanions.Any(p => p.Blueprint == ins.Blueprint))
                {
                    __result = true;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixPrologueBullshit", e); }
        }
    }
}
