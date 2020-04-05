﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.spacepuppy.SPInput.Unity.Xbox.Profiles
{

    /// <summary>
    /// InputName: "XInput Controller"
    /// InputName (per InControl): "Microsoft X-Box 360 pad", "Generic X-Box pad"
    /// </summary>
    [InputProfileDescription("Xbox 360 Controller", TargetPlatform.Linux, Description = "Xbox 360 Controller (Linux)")]
    [InputProfileJoystickName(XboxInputProfile.GENERIC_XBOX360)]
    [InputProfileJoystickName("Microsoft X-Box 360 pad")]
    [InputProfileJoystickName("Generic X-Box pad")]
    public class Xbox360LinuxProfile : XboxInputProfile
    {

        public Xbox360LinuxProfile()
        {
            this.RegisterAxis(XboxInputId.LStickX, SPInputId.Axis1);
            this.RegisterAxis(XboxInputId.LStickY, SPInputId.Axis2, true);
            this.RegisterAxis(XboxInputId.RStickX, SPInputId.Axis4);
            this.RegisterAxis(XboxInputId.RStickY, SPInputId.Axis5, true);
            this.RegisterAxis(XboxInputId.DPadX, SPInputId.Axis7);
            this.RegisterAxis(XboxInputId.DPadY, SPInputId.Axis8, true);
            this.RegisterTrigger(XboxInputId.LTrigger, SPInputId.Axis3);
            this.RegisterTrigger(XboxInputId.RTrigger, SPInputId.Axis6);

            this.RegisterButton(XboxInputId.A, SPInputId.Button0);
            this.RegisterButton(XboxInputId.B, SPInputId.Button1);
            this.RegisterButton(XboxInputId.X, SPInputId.Button2);
            this.RegisterButton(XboxInputId.Y, SPInputId.Button3);
            this.RegisterButton(XboxInputId.LB, SPInputId.Button4);
            this.RegisterButton(XboxInputId.RB, SPInputId.Button5);
            this.RegisterButton(XboxInputId.Back, SPInputId.Button6);
            this.RegisterButton(XboxInputId.Start, SPInputId.Button7);
            this.RegisterButton(XboxInputId.LStickPress, SPInputId.Button9);
            this.RegisterButton(XboxInputId.RStickPress, SPInputId.Button10);
            this.RegisterButton(XboxInputId.DPadRight, SPInputId.Button12);
            this.RegisterButton(XboxInputId.DPadLeft, SPInputId.Button11);
            this.RegisterButton(XboxInputId.DPadUp, SPInputId.Button13);
            this.RegisterButton(XboxInputId.DPadDown, SPInputId.Button14);
            /*
            this.RegisterAxis(XboxInputId.LStickX, SPInputId.Axis1);
            this.RegisterAxis(XboxInputId.LStickY, SPInputId.Axis2, true);
            this.RegisterAxis(XboxInputId.RStickX, SPInputId.Axis4);
            this.RegisterAxis(XboxInputId.RStickY, SPInputId.Axis5);
            this.RegisterAxis(XboxInputId.DPadX, SPInputId.Axis7);
            this.RegisterAxis(XboxInputId.DPadY, SPInputId.Axis8);
            this.RegisterTrigger(XboxInputId.LTrigger, SPInputId.Axis3);
            this.RegisterTrigger(XboxInputId.RTrigger, SPInputId.Axis6);

            this.RegisterButton(XboxInputId.A, SPInputId.Button0);
            this.RegisterButton(XboxInputId.B, SPInputId.Button1);
            this.RegisterButton(XboxInputId.X, SPInputId.Button2);
            this.RegisterButton(XboxInputId.Y, SPInputId.Button3);
            this.RegisterButton(XboxInputId.LB, SPInputId.Button4);
            this.RegisterButton(XboxInputId.RB, SPInputId.Button5);
            this.RegisterButton(XboxInputId.Back, SPInputId.Button6);
            this.RegisterButton(XboxInputId.Start, SPInputId.Button7);
            this.RegisterButton(XboxInputId.LStickPress, SPInputId.Button9);
            this.RegisterButton(XboxInputId.RStickPress, SPInputId.Button10);
            this.RegisterButton(XboxInputId.DPadRight, SPInputId.Button15);
            this.RegisterButton(XboxInputId.DPadLeft, SPInputId.Button16);
            this.RegisterButton(XboxInputId.DPadUp, SPInputId.Button13);
            this.RegisterButton(XboxInputId.DPadDown, SPInputId.Button14);
            */
        }

    }

}
