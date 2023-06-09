namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRZ : AxisButtonCommand
    {
        public AxisRZ()
            : base("RZ Axis")
        {
        }

        protected override int DoCommand(CommandInfoType commandInfo, Joystick joystick, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    joystick.RZ = 0;
                    break;
                case 0:
                    joystick.RZ = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    joystick.RZ = joystick.MaxValue;
                    break;
                default:
                    joystick.RZ += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (joystick.RZ < 0)
                joystick.RZ = 0;
            
            if (joystick.RZ > joystick.MaxValue)
                joystick.RZ = joystick.MaxValue;
            
            joystick.SetAxis(joystick.RZ, HID_USAGES.HID_USAGE_RZ);
            
            return joystick.RZ;
        }
    }
}