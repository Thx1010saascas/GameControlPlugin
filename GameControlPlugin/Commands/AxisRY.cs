namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRY : AxisButtonCommand
    {
        public AxisRY()
            : base("RY Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.RY = 0;
                    break;
                case 0:
                    joystick.RY = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.RY = joystick.MaxValue;
                    break;
                default:
                    joystick.RY += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.RY < 0)
                joystick.RY = 0;

            if (joystick.RY > joystick.MaxValue)
                joystick.RY = joystick.MaxValue;
            
            joystick.SetAxis(joystick.RY, HID_USAGES.HID_USAGE_RY);
        }
    }
}