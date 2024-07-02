using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.UI.MVVM._VM.GroupChanger;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;

namespace PrestigeMinus
{
    [HarmonyPatch(typeof(Recruit), nameof(Recruit.RunAction))]
    internal class EnforceLimitFix1
    {
        static void Postfix()
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                {
                    part.Partysize = 6;
                    return;
                }
                if (Game.Instance.Player.PartyCharacters.Count() > part.Partysize)
                {
                    if (Game.Instance.LoadedAreaState.Settings.CapitalPartyMode)
                    {
                        return;
                    }
                    EventBus.RaiseEvent<IGroupChangerHandler>(delegate (IGroupChangerHandler h)
                    {
                        h.HandleCall(delegate
                        {
                            Game.Instance.Player.FixPartyAfterChange(true);
                            foreach (var unit in Game.Instance.Player.Party)
                            {
                                if (unit == kc) { continue; }
                                unit.Progression.AdvanceExperienceTo(kc.Progression.Experience);
                            }
                        }, delegate
                        {
                            //cancel button
                        }, true, null, null);
                    }, true);
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to EnforceLimitFix1", e); }
        }
    }

    [HarmonyPatch(typeof(CreateCustomCompanion), nameof(CreateCustomCompanion.RunAction))]
    internal class EnforceLimitFix2
    {
        static void Postfix()
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                if (kc.Progression.GetClassLevel(CharacterClassRefs.SwarmThatWalksClass.Reference) > 0)
                {
                    part.Partysize = 6;
                    return;
                }
                if (Game.Instance.Player.PartyCharacters.Count() > part.Partysize)
                {
                    if (Game.Instance.LoadedAreaState.Settings.CapitalPartyMode)
                    {
                        return;
                    }
                    EventBus.RaiseEvent<IGroupChangerHandler>(delegate (IGroupChangerHandler h)
                    {
                        h.HandleCall(delegate
                        {
                            Game.Instance.Player.FixPartyAfterChange(true);
                            foreach (var unit in Game.Instance.Player.Party)
                            {
                                if (unit == kc) { continue; }
                                unit.Progression.AdvanceExperienceTo(kc.Progression.Experience);
                            }
                        }, delegate
                        {
                            //cancel button
                        }, true, null, null);
                    }, true);
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to EnforceLimitFix2", e); }
        }
    }
}
