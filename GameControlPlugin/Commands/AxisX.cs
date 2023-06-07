namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisX : AxisButtonCommand
    {
        public AxisX()
            : base("X Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.X = 0;
                    break;
                case 0:
                    joystick.X = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.X = joystick.MaxValue;
                    break;
                default:
                    joystick.X += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.X < 0)
                joystick.X = 0;
            
            if (joystick.X > joystick.MaxValue)
                joystick.X = joystick.MaxValue;
            
            joystick.SetAxis(joystick.X, HID_USAGES.HID_USAGE_X);
        }
    }
}