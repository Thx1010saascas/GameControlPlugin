namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal class AxisSL1Adjustment : AxisAdjustment
    {
        public AxisSL1Adjustment()
            : base("SL1 Axis", "Adjusts SL1 axis", "SL1 Axis Adjustment")
        {
        }

        protected override void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SL1 += ticks * commandInfo.Value;
            if (joystick.SL1 < 0)
                joystick.SL1 = 0;
            
            if (joystick.SL1 > joystick.MaxValue)
                joystick.SL1 = joystick.MaxValue;
            
            joystick.SetAxis(joystick.SL1, HID_USAGES.HID_USAGE_SL1);
        }

        protected override void RunCommand(string actionParameter)
        {
            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SL1 = joystick.MaxValue / 2;
            
            joystick.SetAxis(joystick.SL1, HID_USAGES.HID_USAGE_SL1);
            
            this.ActionImageChanged(actionParameter);
        }
        
        protected override string GetAdjustmentValue(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);

            if (!commandInfo.DrawNumbers)
                return string.Empty;

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            return GetAdjustmentValue(joystick.SL1, joystick.MaxValue);
        }
    }
}