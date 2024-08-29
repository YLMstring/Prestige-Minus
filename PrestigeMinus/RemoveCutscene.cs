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
            var cut = BlueprintTool.GetRef<CutsceneReference>("a1c7c446015a478eb7c75249a6a783a3");
            cut.Get().m_Tracks.RemoveAt(0);
        }
    }
}
