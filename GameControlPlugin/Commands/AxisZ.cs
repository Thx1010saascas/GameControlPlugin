namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisZ : AxisButtonCommand
    {
        public AxisZ()
            : base("Z Axis")
        {
        }

        protected override int DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.Z = 0;
                    break;
                case 0:
                    joystick.Z = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.Z = joystick.MaxValue;
                    break;
                default:
                    joystick.Z += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.Z < 0)
                joystick.Z = 0;

            if (joystick.Z > joystick.MaxValue)
                joystick.Z = joystick.MaxValue;
            
            joystick.SetAxis(joystick.Z, HID_USAGES.HID_USAGE_Z);
            
            return joystick.X;
        }
    }
}