namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class AxisSL0 : AxisButtonCommand
    {
        public AxisSL0()
            : base("SL0 Axis")
        {
        }

        protected override void DoCommand(CommandInfoType commandInfo, int maxValue, string actionParameter)
        {
            switch (commandInfo.Value)
            {
                case -100:
                    GameControlPlugin.SL0 = 0;
                    break;
                case 0:
                    GameControlPlugin.SL0 = (int)Math.Round(0.5 * maxValue);
                    break;
                case 100:
                    GameControlPlugin.SL0 = maxValue;
                    break;
                default:
                    GameControlPlugin.SL0 += (int)Math.Round(0.01 * maxValue * commandInfo.Value);
                    break;
            }

            if (GameControlPlugin.SL0 < 0)
                GameControlPlugin.SL0 = 0;
            if (GameControlPlugin.SL0 > (int)GameControlPlugin.maxValue)
                GameControlPlugin.SL0 = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL0, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL0);
        }
    }
}