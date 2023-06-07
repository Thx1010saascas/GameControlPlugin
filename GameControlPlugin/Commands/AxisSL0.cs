namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisSL0 : AxisButtonCommand
    {
        public AxisSL0()
            : base("SL0 Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.SL0 = 0;
                    break;
                case 0:
                    joystick.SL0 = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.SL0 = joystick.MaxValue;
                    break;
                default:
                    joystick.SL0 += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.SL0 < 0)
                joystick.SL0 = 0;
            
            if (joystick.SL0 > joystick.MaxValue)
                joystick.SL0 = joystick.MaxValue;
            
            joystick.SetAxis(joystick.SL0, HID_USAGES.HID_USAGE_SL0);
        }
    }
}