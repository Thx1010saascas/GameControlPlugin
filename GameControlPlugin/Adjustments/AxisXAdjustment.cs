namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisXAdjustment : AxisAdjustment
    {
        public AxisXAdjustment()
            : base("X Axis", "Adjusts X axis", "X Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.X += ticks * commandInfo.Value;
            if (GameControlPlugin.X < 0)
                GameControlPlugin.X = 0;
            if (GameControlPlugin.X > (int)GameControlPlugin.maxValue)
                GameControlPlugin.X = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.X, GameControlPlugin.id, HID_USAGES.HID_USAGE_X);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.X = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.X, GameControlPlugin.id, HID_USAGES.HID_USAGE_X);
            this.ActionImageChanged(actionParameter);
        }
    }
}