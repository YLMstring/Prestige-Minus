using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;

namespace PrestigeMinus
{
    internal class MinusAbility
    {
        private static readonly string SuperDisplayName = "MinusMainAbility.Name";
        private static readonly string SuperDescription = "MinusMainAbility.Description";

        private const string SuperAbility = "MinusAbility.SuperAbility";
        private static readonly string SuperAbilityGuid = "{8DAEED2C-92E9-498B-A543-C33C53C05ED9}";

        private const string SuperAbilitybuff = "SuperAbility.SuperAbilitybuff";
        public static readonly string SuperAbilitybuffGuid = "{E6426CE4-FC67-4EA1-A89C-169304336F28}";

        private const string SuperAbility2buff = "SuperAbility.SuperAbility2buff";
        public static readonly string SuperAbility2buffGuid = "{E9386587-79B5-427B-8FD6-43D30D5ADA1E}";
        public static void Configure()
        {
            var icon = AbilityRefs.TricksterTrickFate.Reference.Get().Icon;

            var BuffSuperAbility = BuffConfigurator.New(SuperAbilitybuff, SuperAbilitybuffGuid)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .SetRanks(7800000)
              .Configure();

            var BuffSuperAbility2 = BuffConfigurator.New(SuperAbility2buff, SuperAbility2buffGuid)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .SetRanks(6)
              .Configure();

            var ability = AbilityConfigurator.New(SuperAbility, SuperAbilityGuid)
                .AddComponent(AbilityRefs.Sleep.Reference.Get().GetComponent<AbilitySpawnFx>())
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<MinusShowPartySelection>()
                    .Build())
                .SetDisplayName(SuperDisplayName)
                .SetDescription(SuperDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .AddHideFeatureInInspect()
                .AddComponent<AbilityRequirementKC>()
                .AddAbilityCasterHasNoFacts([FeatureRefs.SwarmFormFeature.ToString()])
                .AddHideDCFromTooltip()
                .Configure();

            FeatureConfigurator.For(FeatureRefs.SkillAbilities)
                    .AddFacts([ability])
                    .Configure();
        }

        private static readonly string SizeUpDisplayName = "MinusSizeUpAbility.Name";
        private static readonly string SizeUpDescription = "MinusSizeUpAbility.Description";

        private const string SizeUpAbility = "MinusAbility.SizeUpAbility";
        private static readonly string SizeUpAbilityGuid = "{A584EF78-C29C-49F8-A256-B77BC1F678A1}";
        public static void Configure2()
        {
            var icon = AbilityRefs.BurstOfGlory.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(SizeUpAbility, SizeUpAbilityGuid)
                .AddComponent(AbilityRefs.BurstOfGlory.Reference.Get().GetComponent<AbilitySpawnFx>())
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<MinusPartySizeUp>()
                    .Build())
                .SetDisplayName(SizeUpDisplayName)
                .SetDescription(SizeUpDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .AddHideFeatureInInspect()
                .AddComponent<AbilityRequirementKC>()
                .AddAbilityCasterHasNoFacts([FeatureRefs.SwarmFormFeature.ToString()])
                .AddHideDCFromTooltip()
                .Configure();

            FeatureConfigurator.For(FeatureRefs.SkillAbilities)
                    .AddFacts([ability])
                    .Configure();
        }
    }
    internal class AbilityRequirementKC : BlueprintComponent, IAbilityCasterRestriction
    {
        // Token: 0x0600D391 RID: 54161 RVA: 0x0036DA08 File Offset: 0x0036BC08
        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            if (caster != Game.Instance.Player.MainCharacter)
            {
                return false;
            }
            return true;
        }

        // Token: 0x0600D392 RID: 54162 RVA: 0x0036DB5C File Offset: 0x0036BD5C
        public string GetAbilityCasterRestrictionUIText()
        {
            return "The ability is for main character only";
        }
    }

    public class MinusShowPartySelection : GameAction
    {
        // Token: 0x0600F521 RID: 62753 RVA: 0x003E255A File Offset: 0x003E075A
        public override string GetCaption()
        {
            return "Show party selection";
        }

        // Token: 0x0600F522 RID: 62754 RVA: 0x003E2561 File Offset: 0x003E0761
        public override void RunAction()
        {
            try
            {
                if (Game.Instance.LoadedAreaState.Settings.CapitalPartyMode)
                {
                    UIUtility.SendWarning("Go out of the safe zone!");
                    return;
                }
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                bool isnew = false;
                if (part.Partysize == 6 && part.Realexp < 3600000)
                {
                    UIUtility.SendWarning("Choose your party size!");
                    isnew = true;
                }
                EventBus.RaiseEvent<IGroupChangerHandler>(delegate (IGroupChangerHandler h)
                {
                    h.HandleCall(delegate
                    {
                        if (isnew)
                        {
                            part.Partysize = Game.Instance.Player.Party.Count();
                            if (part.Partysize == 1)
                            {
                                part.Partysize = 6;
                                UIUtility.SendWarning("Solo is not supported!");
                            }
                            else if (part.Partysize == 6)
                            {
                                UIUtility.SendWarning("Nothing happened!");
                            }
                            else
                            {
                                UIUtility.SendWarning("Your party size is " + part.Partysize.ToString() + ", no going back!");
                            }
                        }
                        Game.Instance.Player.FixPartyAfterChange(true);
                    }, delegate
                    {
                        if (isnew)
                        {
                            UIUtility.SendWarning("Nothing happened!");
                        }
                    }, true, null, null);
                }, true);
            }
            catch (Exception e) { Main.Logger.Error("Failed to MinusShowPartySelection", e); }
        }
    }

    public class MinusPartySizeUp : GameAction
    {
        // Token: 0x0600F521 RID: 62753 RVA: 0x003E255A File Offset: 0x003E075A
        public override string GetCaption()
        {
            return "Minus Party Size Up";
        }

        // Token: 0x0600F522 RID: 62754 RVA: 0x003E2561 File Offset: 0x003E0761
        public override void RunAction()
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                int exp = part.TrySizeUp();
                if (exp == 0)
                {
                    UIUtility.SendWarning("Party size up! Now is " + part.Partysize.ToString());
                }
                else if (exp > 1)
                {
                    UIUtility.SendWarning("Party size is " + part.Partysize.ToString() + ". Raw EXP needed for next party size up: " + exp.ToString());
                }
                else
                {
                    UIUtility.SendWarning("Party size is 6");
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to MinusShowPartySelection", e); }
        }
    }
}
