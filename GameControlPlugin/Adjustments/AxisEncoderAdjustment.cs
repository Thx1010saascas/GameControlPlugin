namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisEncoderAdjustment : AxisAdjustment
    {
        public AxisEncoderAdjustment()
            : base("Encoder", $"Adjusts a axis using an encoder ({GameControlPlugin.AxisNames})", "Named Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            AdjustmentInfo adjustmentInfo = GetAdjustmentInfo(actionParameter, joystick);

            adjustmentInfo.StickValue += ticks * commandInfo.Value;

            if (adjustmentInfo.StickValue < 0)
                adjustmentInfo.StickValue = 0;

            if (adjustmentInfo.StickValue > joystick.MaxValue)
                adjustmentInfo.StickValue = joystick.MaxValue;

            adjustmentInfo.SetStickValue(joystick);
        }
    }
}