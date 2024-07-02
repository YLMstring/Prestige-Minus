using Kingmaker.UnitLogic.Abilities;
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
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers;

namespace PrestigeMinus
{
    internal class UnitPartExpCalculator : UnitPart
    {
        public int Realexp 
        {
            get 
            {
                var fact = Owner.GetFact(RealexpBuff);
                if (fact != null)
                {
                    return fact.GetRank();
                }
                return 0;
            }
            set
            {
                var fact = Owner.GetFact(RealexpBuff) as Buff;
                if (fact != null)
                {
                    fact.SetRank(value);
                }
                else
                {
                    fact = Owner.AddBuff(RealexpBuff, Owner);
                    fact.SetRank(value);
                }
            }
        }

        public int Partysize
        {
            get
            {
                var fact = Owner.GetFact(PartysizeBuff);
                if (fact != null)
                {
                    return fact.GetRank();
                }
                return 6;
            }
            set
            {
                var fact = Owner.GetFact(PartysizeBuff) as Buff;
                if (fact != null)
                {
                    fact.SetRank(value);
                }
                else
                {
                    fact = Owner.AddBuff(PartysizeBuff, Owner);
                    fact.SetRank(value);
                }
            }
        }

        private static readonly BlueprintBuffReference RealexpBuff = BlueprintTool.GetRef<BlueprintBuffReference>(MinusAbility.SuperAbilitybuffGuid);
        private static readonly BlueprintBuffReference PartysizeBuff = BlueprintTool.GetRef<BlueprintBuffReference>(MinusAbility.SuperAbility2buffGuid);
        public int TrySizeUp()
        {
            if (Partysize >= 6) { return -1; }
            int willbesize = Partysize + 1;
            int level20exp = 3600000;
            int expneeded = level20exp * willbesize / 6;
            if (Realexp >= expneeded) 
            {
                Partysize += 1;
                return 0;
            }
            return expneeded - Realexp;
        }
    }

    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.GainExperience))]
    internal class ExpCalculatorFix
    {
        static void Prefix(ref UnitProgressionData __instance, ref int exp)
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                if (kc == __instance.Owner.Unit)
                {
                    Main.Logger.Info("Original exp: " + exp.ToString());
                    var part = kc.Ensure<UnitPartExpCalculator>();
                    part.Realexp += exp;
                    if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                    {
                        part.Partysize = 6;
                        return;
                    }
                    exp = exp * 6 / part.Partysize;
                    Main.Logger.Info("Altered exp: " + exp.ToString());
                    if (SettingsRoot.Difficulty.OnlyActiveCompanionsReceiveExperience || SettingsRoot.Difficulty.OnlyInitiatorReceiveSkillCheckExperience)
                    {
                        exp = 0;
                        UIUtility.SendWarning("Fatal Error!!! Turn owlcat custom experience options off plz");
                    }
                }
                else
                {
                    exp = Math.Min(exp, kc.Progression.Experience - __instance.Owner.Progression.Experience);
                    exp = Math.Max(exp, 0);
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to ExpCalculatorFix1", e); }
            
        }
        static void Postfix(ref UnitProgressionData __instance)
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                {
                    var part = kc.Ensure<UnitPartExpCalculator>();
                    part.Partysize = 6;
                    return;
                }
                if (kc != __instance.Owner.Unit) { return; }
                foreach (var unit in Game.Instance.Player.Party)
                {
                    if (unit == kc) { continue; }
                    unit.Progression.AdvanceExperienceTo(kc.Progression.Experience);
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to ExpCalculatorFix2", e); }
        }
    }
}
