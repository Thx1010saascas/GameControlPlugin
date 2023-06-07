namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisX : AxisButtonCommand
    {
        public AxisX()
            : base("X Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.X = 0;
                    break;
                case 0:
                    GameControlPlugin.X = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.X = maxValue;
                    break;
                default:
                    GameControlPlugin.X += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.X < 0)
                GameControlPlugin.X = 0;
            if (GameControlPlugin.X > (int)GameControlPlugin.maxValue)
                GameControlPlugin.X = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.X, GameControlPlugin.id, HID_USAGES.HID_USAGE_X);
        }
    }
}