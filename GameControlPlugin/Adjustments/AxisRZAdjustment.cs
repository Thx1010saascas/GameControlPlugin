namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisRZAdjustment : AxisAdjustment
    {
        public AxisRZAdjustment()
            : base("RZ Axis", "Adjusts RZ axis", "RZ Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.RZ += ticks * commandInfo.Value;
            if (GameControlPlugin.RZ < 0)
                GameControlPlugin.RZ = 0;
            if (GameControlPlugin.RZ > (int)GameControlPlugin.maxValue)
                GameControlPlugin.RZ = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RZ, GameControlPlugin.id, HID_USAGES.HID_USAGE_RZ);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.RZ = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RZ, GameControlPlugin.id, HID_USAGES.HID_USAGE_RZ);
            this.ActionImageChanged(actionParameter);
        }
    }
}