namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisRX : AxisButtonCommand
    {
        public AxisRX()
            : base("RX Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.RX = 0;
                    break;
                case 0:
                    GameControlPlugin.RX = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.RX = maxValue;
                    break;
                default:
                    GameControlPlugin.RX += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.RX < 0)
                GameControlPlugin.RX = 0;
            if (GameControlPlugin.RX > GameControlPlugin.maxValue)
                GameControlPlugin.RX = (int)GameControlPlugin.maxValue;

            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RX, GameControlPlugin.id, HID_USAGES.HID_USAGE_RX);
        }
    }
}