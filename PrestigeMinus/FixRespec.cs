using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeMinus
{
    [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.MarkAsCloneOfMainCharacter))]
    internal class FixRespec
    {
        static void Postfix(ref UnitEntityData __instance)
        {
            try
            {
                var kc = Game.Instance.Player.MainCharacter.Value;
                var part = kc.Ensure<UnitPartExpCalculator>();
                var part2 = __instance.Ensure<UnitPartExpCalculator>();
                if (part.Realexp > 1)
                {
                    part2.Realexp = part.Realexp - 1;
                }
                if (part.Partysize > 1)
                {
                    part2.Partysize = part.Partysize - 1;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixRespec", e); }
        }
    }
}
