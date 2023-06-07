namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisYAdjustment : AxisAdjustment
    {
        public AxisYAdjustment()
            : base("Y Axis", "Adjusts Y axis", "Y Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.Y += ticks * commandInfo.Value;
            if (GameControlPlugin.Y < 0)
                GameControlPlugin.Y = 0;
            if (GameControlPlugin.Y > (int)GameControlPlugin.maxValue)
                GameControlPlugin.Y = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Y, GameControlPlugin.id, HID_USAGES.HID_USAGE_Y);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.Y = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Y, GameControlPlugin.id, HID_USAGES.HID_USAGE_Y);
            this.ActionImageChanged(actionParameter);
        }
    }
}