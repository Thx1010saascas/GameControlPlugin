namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRY : AxisButtonCommand
    {
        public AxisRY()
            : base("RY Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.RY = 0;
                    break;
                case 0:
                    GameControlPlugin.RY = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.RY = maxValue;
                    break;
                default:
                    GameControlPlugin.RY += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.RY < 0)
                GameControlPlugin.RY = 0;
            if (GameControlPlugin.RY > (int)GameControlPlugin.maxValue)
                GameControlPlugin.RY = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RY, GameControlPlugin.id, HID_USAGES.HID_USAGE_RY);
        }
    }
}