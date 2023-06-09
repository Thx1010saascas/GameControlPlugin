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
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            
            joystick.Z += ticks * commandInfo.Value;
            
            if (joystick.Z < 0)
                joystick.Z = 0;
            
            if (joystick.Z > joystick.MaxValue)
                joystick.Z = joystick.MaxValue;
            
            joystick.SetAxis(joystick.Z, HID_USAGES.HID_USAGE_Z);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.Z = joystick.MaxValue / 2;
            joystick.SetAxis(joystick.Z, HID_USAGES.HID_USAGE_Z);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            if (joystick.Z == int.MinValue)
                joystick.Z = JoystickManager.GetAxisDefaultValue(actionParameter) ?? joystick.MaxValue / 2;
            
            return GetAdjustmentValue(joystick.Z, joystick.MaxValue);
        }
    }
}