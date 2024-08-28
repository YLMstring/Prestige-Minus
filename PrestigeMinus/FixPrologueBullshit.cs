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
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Cond = Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using UnityEngine;
using UnityEngine.Serialization;

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

    [HarmonyPatch(typeof(DialogSpeaker), nameof(DialogSpeaker.GetEntity))]
    internal class FixPrologueBullshit2
    {
        static void Postfix(ref UnitEntityData __result, ref DialogSpeaker __instance)
        {
            try
            {
                if (Game.Instance.Player.Chapter > 0)
                {
                    return;
                }
                if (__result != null)
                {
                    return;
                }
                var ins = __instance;
                if (Game.Instance.Player.RemoteCompanions.Any(p => p.Blueprint == ins.Blueprint))
                {
                    __result = Game.Instance.Player.MainCharacter;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixPrologueBullshit2", e); }
        }
    }

    [HarmonyPatch(typeof(CompanionInParty), nameof(CompanionInParty.GetValueInternal))]
    internal class FixPrologueBullshit3
    {
        static void Postfix(ref UnitEntityData __result, ref CompanionInParty __instance)
        {
            try
            {
                if (Game.Instance.Player.Chapter > 0)
                {
                    return;
                }
                if (__result != null)
                {
                    return;
                }
                var ins = __instance;
                if (Game.Instance.Player.RemoteCompanions.Any(p => p.Blueprint == ins.m_Companion?.Get()))
                {
                    __result = Game.Instance.Player.MainCharacter;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixPrologueBullshit3", e); }
        }
    }

    [HarmonyPatch(typeof(Cond.CompanionInParty), nameof(Cond.CompanionInParty.CheckCondition))]
    internal class FixPrologueBullshit4
    {
        static void Postfix(ref bool __result, ref Cond.CompanionInParty __instance)
        {
            try
            {
                if (Game.Instance.Player.Chapter > 0)
                {
                    return;
                }
                var ins = __instance;
                if (__result || !ins.MatchWhenActive || ins.MatchWhenRemote)
                {
                    return;
                }
                if (Game.Instance.Player.RemoteCompanions.Any(p => p.Blueprint == ins.m_companion?.Get()))
                {
                    __result = true;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixPrologueBullshit4", e); }
        }
    }
}
