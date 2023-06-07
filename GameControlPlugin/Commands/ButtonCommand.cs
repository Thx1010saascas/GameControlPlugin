namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal abstract class ButtonCommand : PluginDynamicCommand
    {
        protected ButtonCommand(string displayName, string action)
        {
            this.DisplayName = displayName;
            this.GroupName = "Not used";
            this.MakeProfileAction(action);
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
                if (commandInfo.DrawNumbers)
                    bitmapBuilder.DrawText(string.Format("{0}", commandInfo.Value), fontSize: 20);
                if (commandInfo.Label != "")
                {
                    bitmapBuilder.FillRectangle(0, (int)(commandInfo.LabelPos - Math.Round(commandInfo.LabelSize * commandInfo.LabelSize / 28.0)), 80, commandInfo.LabelSize, commandInfo.LabelBackgroundColor);
                    bitmapBuilder.DrawText(commandInfo.Label ?? "", 0, commandInfo.LabelPos - 14, 80, commandInfo.LabelSize + 6, commandInfo.LabelColor, commandInfo.LabelSize, 18);
                }

                return bitmapBuilder.ToImage();
            }
        }
    }
}