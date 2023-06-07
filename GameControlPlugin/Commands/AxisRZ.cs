namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRZ : AxisButtonCommand
    {
        public AxisRZ()
            : base("RZ Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.RZ = 0;
                    break;
                case 0:
                    GameControlPlugin.RZ = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.RZ = maxValue;
                    break;
                default:
                    GameControlPlugin.RZ += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.RZ < 0)
                GameControlPlugin.RZ = 0;
            if (GameControlPlugin.RZ > (int)GameControlPlugin.maxValue)
                GameControlPlugin.RZ = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RZ, GameControlPlugin.id, HID_USAGES.HID_USAGE_RZ);
        }
    }
}