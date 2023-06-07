namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisRYAdjustment : AxisAdjustment
    {
        public AxisRYAdjustment()
            : base("RY Axis", "Adjusts RY axis", "RY Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.RY += ticks * commandInfo.Value;
            if (GameControlPlugin.RY < 0)
                GameControlPlugin.RY = 0;
            if (GameControlPlugin.RY > (int)GameControlPlugin.maxValue)
                GameControlPlugin.RY = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RY, GameControlPlugin.id, HID_USAGES.HID_USAGE_RY);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.RY = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RY, GameControlPlugin.id, HID_USAGES.HID_USAGE_RY);
            this.ActionImageChanged(actionParameter);
        }
    }
}