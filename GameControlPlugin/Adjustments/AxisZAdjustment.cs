namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisZAdjustment : AxisAdjustment
    {
        public AxisZAdjustment()
            : base("Z Axis", "Adjusts Z axis", "Z Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.Z += ticks * commandInfo.Value;
            if (GameControlPlugin.Z < 0)
                GameControlPlugin.Z = 0;
            if (GameControlPlugin.Z > (int)GameControlPlugin.maxValue)
                GameControlPlugin.Z = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Z, GameControlPlugin.id, HID_USAGES.HID_USAGE_Z);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.Z = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Z, GameControlPlugin.id, HID_USAGES.HID_USAGE_Z);
            this.ActionImageChanged(actionParameter);
        }
    }
}