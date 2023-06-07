namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisSL0Adjustment : AxisAdjustment
    {
        public AxisSL0Adjustment()
            : base("SL0 Axis", "Adjusts SL0 axis", "SL0 Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SL0 += ticks * commandInfo.Value;
            
            if (joystick.SL0 < 0)
                joystick.SL0 = 0;
            
            if (joystick.SL0 > joystick.MaxValue)
                joystick.SL0 = joystick.MaxValue;
            
            joystick.SetAxis(joystick.SL0, HID_USAGES.HID_USAGE_SL0);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SL0 = joystick.MaxValue / 2;
            
            joystick.SetAxis(joystick.SL0, HID_USAGES.HID_USAGE_SL0);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.SL0, joystick.MaxValue);
        }
    }
}