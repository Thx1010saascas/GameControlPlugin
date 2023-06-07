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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            
            joystick.RY += ticks * commandInfo.Value;
            if (joystick.RY < 0)
                joystick.RY = 0;
            
            if (joystick.RY > joystick.MaxValue)
                joystick.RY = joystick.MaxValue;
            
            joystick.SetAxis(joystick.RY, HID_USAGES.HID_USAGE_RY);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.RY = joystick.MaxValue / 2;
            
            joystick.SetAxis(joystick.RY, HID_USAGES.HID_USAGE_RY);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.RY, joystick.MaxValue);
        }
    }
}