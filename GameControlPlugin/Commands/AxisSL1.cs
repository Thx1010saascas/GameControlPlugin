namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisSL1 : AxisButtonCommand
    {
        public AxisSL1()
            : base("SL1 Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.SL1 = 0;
                    break;
                case 0:
                    GameControlPlugin.SL1 = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.SL1 = maxValue;
                    break;
                default:
                    GameControlPlugin.SL1 += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.SL1 < 0)
                GameControlPlugin.SL1 = 0;
            if (GameControlPlugin.SL1 > (int)GameControlPlugin.maxValue)
                GameControlPlugin.SL1 = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL1, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL1);
        }
    }
}