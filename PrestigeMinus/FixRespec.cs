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
                part2.Realexp = part.Realexp;
                part2.Partysize = part.Partysize;
            }
            catch (Exception e) { Main.Logger.Error("Failed to FixRespec", e); }
        }
    }
}
