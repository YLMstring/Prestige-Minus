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

namespace PrestigeMinus
{
    internal class MinusAbility
    {
        private static readonly string StyleDisplayName = "MinusAbility.Name";
        private static readonly string StyleDescription = "MinusAbility.Description";

        private const string SuperAbility = "MinusAbility.SuperAbility";
        private static readonly string SuperAbilityGuid = "{8DAEED2C-92E9-498B-A543-C33C53C05ED9}";
        public static void StyleConfigure()
        {
            var icon = AbilityRefs.TricksterTrickFate.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(SuperAbility, SuperAbilityGuid)
                .CopyFrom(
                AbilityRefs.Sleep,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<MinusShowPartySelection>()
                    .Build())
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .AddHideFeatureInInspect()
                .AddComponent<AbilityRequirementKC>()
                .AddAbilityCasterHasNoFacts([FeatureRefs.SwarmFormFeature.ToString()])
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
                    UIUtility.SendWarning("Go out of the safe area!");
                    return;
                }
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Get<UnitPartExpCalculator>();
                bool isnew = false;
                if (part.partysize == 6 && part.realexp < 3600000)
                {
                    UIUtility.SendWarning("Choose your party size!");
                    isnew = true;
                }
                else
                {
                    int exp = part.TrySizeUp();
                    if (exp == 0)
                    {
                        UIUtility.SendWarning("Party size up! Now is " + part.partysize.ToString());
                    }
                    else if (exp > 1)
                    {
                        UIUtility.SendWarning("Party size is " + part.partysize.ToString() + ". EXP needed for party size up: " + exp.ToString());
                    }
                    else
                    {
                        UIUtility.SendWarning("Party size is 6");
                    }
                }
                EventBus.RaiseEvent<IGroupChangerHandler>(delegate (IGroupChangerHandler h)
                {
                    h.HandleCall(delegate
                    {
                        Game.Instance.Player.FixPartyAfterChange(true);
                        if (isnew)
                        {
                            part.partysize = Game.Instance.Player.Party.Count();
                            if (part.partysize == 1)
                            {
                                part.partysize = 6;
                                UIUtility.SendWarning("Solo mode is not supported!");
                            }
                            else if (part.partysize == 6)
                            {
                                UIUtility.SendWarning("Nothing happened!");
                            }
                            else
                            {
                                UIUtility.SendWarning("Your party size is " + part.partysize.ToString() + ", no going back!");
                            }
                        }
                        else if (Game.Instance.Player.Party.Count() > part.partysize)
                        {
                            UIUtility.SendWarning("Party too big!");
                            this.RunAction();
                        }
                    }, delegate
                    {
                        if (isnew)
                        {
                            UIUtility.SendWarning("Nothing happened!");
                        }
                        else if (Game.Instance.Player.Party.Count() > part.partysize)
                        {
                            UIUtility.SendWarning("Party too big!");
                            this.RunAction();
                        }
                    }, true, null, null);
                }, true);
            }
            catch (Exception e) { Main.Logger.Error("Failed to MinusShowPartySelection", e); }
        }
    }
}
