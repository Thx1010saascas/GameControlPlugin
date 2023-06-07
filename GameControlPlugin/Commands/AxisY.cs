namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisY : AxisButtonCommand
    {
        public AxisY()
            : base("Y Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.Y = 0;
                    break;
                case 0:
                    GameControlPlugin.Y = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.Y = maxValue;
                    break;
                default:
                    GameControlPlugin.Y += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.Y < 0)
                GameControlPlugin.Y = 0;
            
            if (GameControlPlugin.Y > GameControlPlugin.maxValue)
                GameControlPlugin.Y = (int)GameControlPlugin.maxValue;
            
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Y, GameControlPlugin.id, HID_USAGES.HID_USAGE_Y);
        }
    }
}