namespace Loupedeck.GameControlPlugin.Commands
{
    internal class ButtonToggleOnOffOn : ButtonCommand
    {
        public ButtonToggleOnOffOn()
            : base("Toggles (On-Off-On)", "text;Enter the first dx button in the toggle (1-127) and any options:")
        {
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

            if (GameControlPlugin.Buttons[commandInfo.Value])
            {
                GameControlPlugin.Buttons[commandInfo.Value] = false;
                GameControlPlugin.Buttons[commandInfo.Value + 1] = false;
            }
            else if (GameControlPlugin.Buttons[commandInfo.Value + 1])
            {
                GameControlPlugin.Buttons[commandInfo.Value] = true;
                GameControlPlugin.Buttons[commandInfo.Value + 1] = false;
            }
            else
            {
                GameControlPlugin.Buttons[commandInfo.Value] = false;
                GameControlPlugin.Buttons[commandInfo.Value + 1] = true;
            }

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SetBtn(GameControlPlugin.Buttons[commandInfo.Value], (uint)commandInfo.Value);
            joystick.SetBtn(GameControlPlugin.Buttons[commandInfo.Value + 1], (uint)(commandInfo.Value + 1));
            
            this.ActionImageChanged(actionParameter);
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
                if (GameControlPlugin.Buttons[commandInfo.Value])
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOffOnUpResourcePath : GameControlPlugin.ToggleUpResourcePath));
                else if (GameControlPlugin.Buttons[commandInfo.Value + 1])
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOffOnDownResourcePath : GameControlPlugin.ToggleDownResourcePath));
                else
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOffOnMiddleResourcePath : GameControlPlugin.ToggleMiddleResourcePath));
                if (commandInfo.DrawNumbers)
                    bitmapBuilder.DrawText($"{commandInfo.Value}\n \n{commandInfo.Value + 1}", -3, -5, 20, 80, fontSize: 10, lineHeight: 18);
                
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