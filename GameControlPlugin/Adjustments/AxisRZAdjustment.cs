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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.RZ += ticks * commandInfo.Value;
            
            if (joystick.RZ < 0)
                joystick.RZ = 0;
            
            if (joystick.RZ > joystick.MaxValue)
                joystick.RZ = joystick.MaxValue;
            
            joystick.SetAxis(joystick.RZ, HID_USAGES.HID_USAGE_RZ);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.RZ = joystick.MaxValue / 2;
            
            joystick.SetAxis(joystick.RZ, HID_USAGES.HID_USAGE_RZ);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.RZ, joystick.MaxValue);
        }
    }
}