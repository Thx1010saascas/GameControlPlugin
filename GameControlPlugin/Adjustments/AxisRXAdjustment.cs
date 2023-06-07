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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.RX += ticks * commandInfo.Value;
            
            if (joystick.RX < 0)
                joystick.RX = 0;
            
            if (joystick.RX > joystick.MaxValue)
                joystick.RX = joystick.MaxValue;
            
            joystick.SetAxis(joystick.RX, HID_USAGES.HID_USAGE_RX);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.RX = joystick.MaxValue / 2;
            joystick.SetAxis(joystick.RX, HID_USAGES.HID_USAGE_RX);
            
            this.ActionImageChanged(actionParameter);
        }

        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.RX, joystick.MaxValue);
        }
    }
}