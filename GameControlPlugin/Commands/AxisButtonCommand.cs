namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal abstract class AxisButtonCommand : PluginDynamicCommand
    {
        protected AxisButtonCommand(string displayName)
        {
            this.DisplayName = $"{displayName} (Button)";
            this.GroupName = "Not used";
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

            int value = DoCommand(commandInfo, joystick, actionParameter);
            
            string axisName = this.DisplayName.Substring(0, this.DisplayName.StartsWith("SL") ? 3 : 2).TrimEnd();
            
            // We may have to use the above value to refresh the axis label with the correct new value. But this does what I want for the moment.  
            this.Plugin.ExecuteGenericAction($"Loupedeck.GameControlPlugin.Adjustments.Axis{axisName}Adjustment", "", 1);

            this.ActionImageChanged(actionParameter);
        }

        protected abstract int DoCommand(CommandInfoType commandInfo, Joystick joystick, String actionParameter);

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
    }
}