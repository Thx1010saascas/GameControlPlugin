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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.Y += ticks * commandInfo.Value;
            
            if (joystick.Y < 0)
                joystick.Y = 0;
            
            if (joystick.Y > joystick.MaxValue)
                joystick.Y = joystick.MaxValue;
            
            joystick.SetAxis(joystick.Y, HID_USAGES.HID_USAGE_Y);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.Y = joystick.MaxValue / 2;
            joystick.SetAxis(joystick.Y, HID_USAGES.HID_USAGE_Y);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.Y, joystick.MaxValue);
        }
    }
}