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
                    890000, 1300000, 1800000, 2550000, 3600000, 5700000, 9900000, 14100000, 18300000, 22500000])
                .Configure();

            StatProgressionConfigurator.For(StatProgressionRefs.LegendXPTable)
                .SetBonuses([0, 0, 2000, 5000, 9000, 15000, 23000, 35000, 51000, 55000, 62000, 68000, 75000, 85000, 96000, 105000,
                    115000, 130000, 155000, 180000, 200000, 220000, 260000, 280000, 315000, 370000, 445000, 500000, 635000, 720000,
                    890000, 1000000, 1300000, 1550000, 1800000, 2000000, 2550000, 3000000, 3600000, 4050000, 4700000,
                    5700000, 9900000, 14100000, 18300000, 22500000])
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
                if (__result == 20)
                {
                    __result = 25;
                }
                else if (__result == 40)
                {
                    __result = 45;
                }
            }
            catch (Exception e) { Main.Logger.Error("Failed to Above20Fix", e); }
        }
    }
}
