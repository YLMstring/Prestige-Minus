using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.Settings;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UI.MVVM._VM.GroupChanger;

namespace PrestigeMinus
{
    [HarmonyPatch(typeof(GroupChangerCommonVM), nameof(GroupChangerCommonVM.GoCondition))]
    internal class PartyLimitFix1
    {
        static void Postfix(ref GroupChangerCommonVM __instance, ref bool __result)
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                {
                    part.partysize = 6;
                    return;
                }
                if (__instance.PartyCharacterRef.Count() > part.partysize)
                {
                    __result = false;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to PartyLimitFix1", e); }
        }
    }

    [HarmonyPatch(typeof(GroupChangerCommonVM), nameof(GroupChangerCommonVM.CloseCondition))]
    internal class PartyLimitFix2
    {
        static void Postfix(ref GroupChangerCommonVM __instance, ref bool __result)
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                {
                    part.partysize = 6;
                    return;
                }
                if (__instance.PartyCharacterRef.Count() > part.partysize)
                {
                    __result = false;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to PartyLimitFix2", e); }
        }
    }
}
