namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisSL0Adjustment : AxisAdjustment
    {
        public AxisSL0Adjustment()
            : base("SL0 Axis", "Adjusts SL0 axis", "SL0 Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.SL0 += ticks * commandInfo.Value;
            if (GameControlPlugin.SL0 < 0)
                GameControlPlugin.SL0 = 0;
            if (GameControlPlugin.SL0 > (int)GameControlPlugin.maxValue)
                GameControlPlugin.SL0 = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL0, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL0);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.SL0 = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.SL0, GameControlPlugin.id, HID_USAGES.HID_USAGE_SL0);
            this.ActionImageChanged(actionParameter);
        }
    }
}