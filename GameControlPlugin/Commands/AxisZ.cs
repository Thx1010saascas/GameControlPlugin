namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisZ : AxisButtonCommand
    {
        public AxisZ()
            : base("Z Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.Z = 0;
                    break;
                case 0:
                    GameControlPlugin.Z = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.Z = maxValue;
                    break;
                default:
                    GameControlPlugin.Z += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.Z < 0)
                GameControlPlugin.Z = 0;
            if (GameControlPlugin.Z > (int)GameControlPlugin.maxValue)
                GameControlPlugin.Z = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Z, GameControlPlugin.id, HID_USAGES.HID_USAGE_Z);
        }
    }
}