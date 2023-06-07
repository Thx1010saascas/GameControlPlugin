namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisY : AxisButtonCommand
    {
        public AxisY()
            : base("Y Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.Y = 0;
                    break;
                case 0:
                    joystick.Y = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.Y = joystick.MaxValue;
                    break;
                default:
                    joystick.Y += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.Y < 0)
                joystick.Y = 0;
            
            if (joystick.Y > joystick.MaxValue)
                joystick.Y = joystick.MaxValue;
            
            joystick.SetAxis(joystick.Y, HID_USAGES.HID_USAGE_Y);
        }
    }
}