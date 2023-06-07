namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRX : AxisButtonCommand
    {
        public AxisRX()
            : base("RX Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.RX = 0;
                    break;
                case 0:
                    joystick.RX = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.RX = joystick.MaxValue;
                    break;
                default:
                    joystick.RX += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.RX < 0)
                joystick.RX = 0;
            
            if (joystick.RX > joystick.MaxValue)
                joystick.RX = joystick.MaxValue;

            joystick.SetAxis(joystick.RX, HID_USAGES.HID_USAGE_RX);
        }
    }
}