using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.UI.MVVM._VM.GroupChanger;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;

namespace PrestigeMinus
{
    internal class Above20
    {
        public static void Patch()
        {
            StatProgressionConfigurator.For(StatProgressionRefs.XPTable)
                .SetBonuses([0, 0, 2000, 5000, 9000, 15000, 23000, 35000, 51000, 75000, 105000, 155000, 220000, 315000, 445000, 635000,
                    890000, 1300000, 1800000, 2550000, 3600000, 5700000, 7800000, 9900000, 12000000])
                .Configure();
        }
    }

    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.MaxCharacterLevel), MethodType.Getter)]
    internal class Above20Fix
    {
        static void Postfix(ref int __result)
        {
            try
            {
                if (__result < 24)
                {
                    __result = 24;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to Above20Fix", e); }
        }
    }
}
