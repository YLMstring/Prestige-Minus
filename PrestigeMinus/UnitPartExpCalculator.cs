﻿using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker;
using BlueprintCore.Blueprints.References;
using Kingmaker.Settings;
using Kingmaker.UI.Common;

namespace PrestigeMinus
{
    internal class UnitPartExpCalculator : OldStyleUnitPart
    {
        public int realexp = 0;

        public int partysize = 6;
        public void TrySizeUp()
        {
            if (partysize >= 6) { return; }
            int willbesize = partysize + 1;
            int level20exp = 3600000;
            int expneeded = level20exp * willbesize / 6;
            if (realexp >= expneeded) 
            {
                partysize += 1;
            }
        }
    }

    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.GainExperience))]
    internal class ExpCalculatorFix
    {
        static void Prefix(ref UnitProgressionData __instance, ref int exp)
        {
            var kc = Game.Instance.Player.MainCharacter.Value;
            if (kc == __instance.Owner.Unit)
            {
                var part = kc.Get<UnitPartExpCalculator>();
                part.realexp += exp;
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0) { return; }
                exp = exp * part.partysize / 6;
                if (SettingsRoot.Difficulty.OnlyActiveCompanionsReceiveExperience || SettingsRoot.Difficulty.OnlyInitiatorReceiveSkillCheckExperience)
                {
                    exp = 1;
                    UIUtility.SendWarning("Fatal Error, turn owlcat custom experience options off plz");
                }
            }
            else
            {
                exp = Math.Min(exp, kc.Progression.Experience - __instance.Owner.Progression.Experience);
                exp = Math.Max(exp, 1);
            }
        }

        static void Postfix(ref UnitProgressionData __instance)
        {
            var kc = Game.Instance.Player.MainCharacter.Value;
            if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0) { return; }
            if (kc != __instance.Owner.Unit) { return; }
            foreach (var unit in Game.Instance.Player.Party)
            {
                if (unit == kc) { continue; }
                unit.Progression.AdvanceExperienceTo(kc.Progression.Experience);
            }
        }
    }
}
