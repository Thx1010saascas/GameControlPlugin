namespace Loupedeck.GameControlPlugin.Adjustments
{
    using Commands;

    internal class AxisButtonAdjustment : AxisAdjustment
    {
        public AxisButtonAdjustment()
            : base("Encoder via Button Presses", $"Adjusts an axis using buttons ({GameControlPlugin.AxisNames})", "Button Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            AdjustmentInfo adjustmentInfo = AxisEncoderAdjustment.GetAdjustmentInfo(actionParameter, joystick);

            adjustmentInfo.StickValue += ticks * commandInfo.Value;

            if (adjustmentInfo.StickValue < 0)
                adjustmentInfo.StickValue = 0;

            if (adjustmentInfo.StickValue > joystick.MaxValue)
                adjustmentInfo.StickValue = joystick.MaxValue;

            adjustmentInfo.SetStickValue(joystick);

            if (ticks > 0 && adjustmentInfo.UpButtonId.HasValue)
            {
                this.Plugin.ExecuteGenericAction(typeof(ButtonPress).FullName, $"{actionParameter};{adjustmentInfo.UpButtonId}", 1);
            }
            else
            {
                if (ticks < 0 && adjustmentInfo.DownButtonId.HasValue)
                {
                    this.Plugin.ExecuteGenericAction(typeof(ButtonPress).FullName, $"{actionParameter};{adjustmentInfo.DownButtonId}", 1);
                }
            }
        }
    }
}