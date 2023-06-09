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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.X += ticks * commandInfo.Value;
            
            if (joystick.X < 0)
                joystick.X = 0;
            
            if (joystick.X > joystick.MaxValue)
                joystick.X = joystick.MaxValue;
            
            joystick.SetAxis(joystick.X, HID_USAGES.HID_USAGE_X);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.X = joystick.MaxValue / 2;
            
            joystick.SetAxis(joystick.X, HID_USAGES.HID_USAGE_X);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            if (joystick.X == int.MinValue)
                joystick.X = JoystickManager.GetAxisDefaultValue(actionParameter) ?? joystick.MaxValue / 2;

            return GetAdjustmentValue(joystick.X, joystick.MaxValue);
        }
    }
}