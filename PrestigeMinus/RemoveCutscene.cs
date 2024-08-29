using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeMinus
{
    internal class RemoveCutscene
    {
        public static void Configure()
        {
            var cut = BlueprintTool.GetRef<CutsceneReference>("e02e5defabc90454ea348e7fdce7ea25");
            cut.Get().Priority = Kingmaker.AreaLogic.Cutscenes.CutscenePriority.Reaction;
        }
    }
}
