namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisRXAdjustment : AxisAdjustment
    {
        public AxisRXAdjustment()
            : base("RX Axis", "Adjusts RX axis", "RX Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            GameControlPlugin.RX += ticks * commandInfo.Value;
            if (GameControlPlugin.RX < 0)
                GameControlPlugin.RX = 0;
            if (GameControlPlugin.RX > (int)GameControlPlugin.maxValue)
                GameControlPlugin.RX = (int)GameControlPlugin.maxValue;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RX, GameControlPlugin.id, HID_USAGES.HID_USAGE_RX);
        }

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.RX = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.RX, GameControlPlugin.id, HID_USAGES.HID_USAGE_RX);
            this.ActionImageChanged(actionParameter);
        }
    }
}