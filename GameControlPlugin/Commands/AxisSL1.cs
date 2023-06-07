namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisSL1 : AxisButtonCommand
    {
        public AxisSL1()
            : base("SL1 Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.SL1 = 0;
                    break;
                case 0:
                    joystick.SL1 = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.SL1 = joystick.MaxValue;
                    break;
                default:
                    joystick.SL1 += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.SL1 < 0)
                joystick.SL1 = 0;
            if (joystick.SL1 > joystick.MaxValue)
                joystick.SL1 = joystick.MaxValue;
            joystick.SetAxis(joystick.SL1, HID_USAGES.HID_USAGE_SL1);
        }
    }
}