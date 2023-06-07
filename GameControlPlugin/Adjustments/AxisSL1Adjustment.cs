namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisSL1Adjustment : AxisAdjustment
    {
        public AxisSL1Adjustment()
            : base("SL1 Axis", "Adjusts SL1 axis", "SL1 Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.SL1 += ticks * commandInfo.Value;
            if (GameControlPlugin.SL1 < 0)
                GameControlPlugin.SL1 = 0;
            if (GameControlPlugin.SL1 > (int)GameControlPlugin.maxValue)
                GameControlPlugin.SL1 = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL1, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL1);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.SL1 = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL1, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL1);
            this.ActionImageChanged(actionParameter);
        }
    }
}