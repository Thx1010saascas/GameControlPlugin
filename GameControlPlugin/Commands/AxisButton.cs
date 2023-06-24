namespace Loupedeck.GameControlPlugin.Commands
{
    using System;
    using System.Reflection;
    using Adjustments;

    using Helpers;

    internal class AxisButton : PluginDynamicCommand
    {
        
        public AxisButton()
        {
            this.DisplayName = $"Axis Button";
            this.GroupName = "Not used";
            this.Description = $"Axis push button ({GameControlPlugin.AxisNames})";
            this.MakeProfileAction("text;Enter the amount to change the axis (-100..100, Min, Max or Center) and any options:");
        }

        protected override void RunCommand(string actionParameter)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);
            if (GameControlPlugin.PluginError != "")
                this.Plugin.OnPluginStatusChanged(PluginStatus.Error, GameControlPlugin.PluginError);
            
            if (GameControlPlugin.PluginWarning != "" && !GameControlPlugin.InWarning)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Warning, GameControlPlugin.PluginWarning);
                GameControlPlugin.PluginWarning = "";
                GameControlPlugin.InWarning = true;
            }

            if (GameControlPlugin.InWarning && GameControlPlugin.PluginWarningStopwatch.ElapsedMilliseconds > 2000L)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Normal, null);
                GameControlPlugin.InWarning = false;
            }

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            ButtonInfo buttonInfo = GetButtonInfo(actionParameter, joystick);

            int value = DoCommand(commandInfo, joystick, buttonInfo);
            
            AdjustmentCacheEntry adjustmentCacheEntry = AdjustmentCache.Get(joystick.Id, buttonInfo.AxisName);
            
            // We may have to use the above value to refresh the axis label with the correct new value. But this does what I want for the moment.  
            if(adjustmentCacheEntry != null)
                this.Plugin.ExecuteGenericAction(adjustmentCacheEntry.PluginName, adjustmentCacheEntry.ActionParameter, int.MaxValue);
        }


        protected string GetAdjustmentValue(int value, int maxValue)
        {
            if (GameControlPlugin.PluginError != "")
                this.Plugin.OnPluginStatusChanged(PluginStatus.Error, GameControlPlugin.PluginError);
            
            if (GameControlPlugin.PluginWarning != "" && !GameControlPlugin.InWarning)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Warning, GameControlPlugin.PluginWarning);
                GameControlPlugin.PluginWarning = "";
                GameControlPlugin.InWarning = true;
            }

            if (GameControlPlugin.InWarning && GameControlPlugin.PluginWarningStopwatch.ElapsedMilliseconds > 2000L)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Normal, null);
                GameControlPlugin.InWarning = false;
            }

            return ((int)((value / (double)maxValue * 2.0 - 1.0) * 100.0)) + "%";
        }

        protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            CommandInfoType commandInfo = GameControlPlugin.GetCommandInfo(actionParameter);
            
            if (GameControlPlugin.PluginError != "")
                this.Plugin.OnPluginStatusChanged(PluginStatus.Error, GameControlPlugin.PluginError);
            
            if (GameControlPlugin.PluginWarning != "" && !GameControlPlugin.InWarning)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Warning, GameControlPlugin.PluginWarning);
                GameControlPlugin.PluginWarning = "";
                GameControlPlugin.InWarning = true;
            }

            if (GameControlPlugin.InWarning && GameControlPlugin.PluginWarningStopwatch.ElapsedMilliseconds > 2000L)
            {
                this.Plugin.OnPluginStatusChanged(PluginStatus.Normal, null);
                GameControlPlugin.InWarning = false;
            }

            using (BitmapBuilder bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.ButtonPath));
                
                if (commandInfo.Label != "")
                {
                    bitmapBuilder.FillRectangle(0, commandInfo.LabelPos - commandInfo.LabelSize / 2, 80, commandInfo.LabelSize, commandInfo.LabelBackgroundColor);
                    bitmapBuilder.DrawText(commandInfo.Label ?? "", 0, commandInfo.LabelPos - 14, 80, commandInfo.LabelSize + 6, commandInfo.LabelColor, commandInfo.LabelSize, 18);
                }

                return bitmapBuilder.ToImage();
            }
        }

        private static int DoCommand(CommandInfoType commandInfo, Joystick joystick, ButtonInfo buttonInfo)
        {
            int newValue = 0;
            
            switch (commandInfo.Value)
            {
                case -100:
                    newValue = 0;
                    break;
                case 0:
                    newValue = (int)Math.Round(0.5 * joystick.MaxValue);
                    break;
                case 100:
                    newValue = joystick.MaxValue;
                    break;
                default:
                    newValue += (int)Math.Round(0.01 * joystick.MaxValue * commandInfo.Value);
                    break;
            }

            if (newValue < 0)
                newValue = 0;
            
            if (newValue > joystick.MaxValue)
                newValue = joystick.MaxValue;

            buttonInfo.SetButtonValue(joystick, newValue);
            
            return newValue;
        }

        private static ButtonInfo GetButtonInfo(string actionParameter, Joystick joystick)
        {
            ButtonInfo buttonInfo = new ButtonInfo();
            
            foreach(string entry in actionParameter.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] values = entry.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (values.Length < 2)
                    continue;

                switch (values[0].Trim().ToLower())
                {
                    case "axis":
                        buttonInfo.SetButtonName(joystick, values[1].Trim().ToUpper());
                        break;
                }
            }
            
            return buttonInfo;
        }
    }
    
    internal class ButtonInfo
    {
        private PropertyInfo _stickAxisProperty;
        private HID_USAGES _hidUsages;

        public string AxisName {get; private set; }
    
        public void SetButtonName(Joystick joystick, string buttonName)
        {
            if (string.IsNullOrWhiteSpace(buttonName))
                throw new Exception($"The button name was missing. Must set 'axis=<{GameControlPlugin.AxisNames}>'.");
            
            AxisName = buttonName;
            _stickAxisProperty = joystick.GetType().GetProperty(buttonName);
   
            if(!Enum.TryParse($"HID_USAGE_{buttonName}", out _hidUsages))
                throw new Exception($"The button name '{buttonName}' is invalid.");
        }

        internal void SetButtonValue(Joystick joystick, int value)
        {
            _stickAxisProperty?.SetValue(joystick, value);

            joystick.SetAxis(value, _hidUsages);
        }
    }
}