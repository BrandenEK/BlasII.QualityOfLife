using BlasII.ModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.QualityOfLife.Modules;

internal class QuickSwapAlts_9 : BaseModule
{
    public override void OnUpdate()
    {
        if (Main.QualityOfLife.InputHandler.GetButton(ModdingAPI.Input.ButtonType.Interact))
            ModLog.Error("Hold interact");
    }
}
