namespace Loupedeck.GameControlPlugin.Adjustments
{
    internal abstract class AxisAdjustment : PluginDynamicAdjustment
    {
        public AxisAdjustment(string displayName, string description, string groupName)
            : base(displayName, description, groupName, false)
        {
            this.DisplayName = $"{displayName} (Rotary Adjustment)";
            this.GroupName = "Not used";
            this.MakeProfileAction("text;Enter the adjustment speed (Normal, Fast, Slow, [1-1000]) and any options:");
        }

        protected override void ApplyAdjustment(string actionParameter, int ticks)
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

            DoAdjustment(GameControlPlugin.GetCommandInfo(actionParameter), actionParameter, ticks);

            this.ActionImageChanged(actionParameter);
        }

        protected abstract void DoAdjustment(CommandInfoType commandInfo, string actionParameter, int ticks);

        protected override void RunCommand(string actionParameter)
        {
            GameControlPlugin.Y = (int)GameControlPlugin.maxValue / 2;
            GameControlPlugin.joystick.SetAxis(GameControlPlugin.Y, GameControlPlugin.id, HID_USAGES.HID_USAGE_Y);
            this.ActionImageChanged(actionParameter);
        }

        protected override string GetAdjustmentValue(string actionParameter)
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

            return commandInfo.DrawNumbers ? ((int)((GameControlPlugin.Y / (double)GameControlPlugin.maxValue * 2.0 - 1.0) * 100.0)) + "%" : "";
        }

        protected override BitmapImage GetCommandImage(
            string actionParameter,
            PluginImageSize imageSize)
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
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.RotaryPath));
                if (commandInfo.Label != "")
                {
                    bitmapBuilder.FillRectangle(0, (int)(0.6 * (commandInfo.LabelPos + 2)) - commandInfo.LabelSize / 2, 50, commandInfo.LabelSize, commandInfo.LabelBackgroundColor);
                    bitmapBuilder.DrawText(commandInfo.Label ?? "", 0, (int)(0.6 * (commandInfo.LabelPos + 2)) - 14, 50, commandInfo.LabelSize + 6, commandInfo.LabelColor, commandInfo.LabelSize, 18);
                }

                return bitmapBuilder.ToImage();
            }
        }
    }
}