namespace Loupedeck.GameControlPlugin.Commands
{
    using System.Threading.Tasks;

    internal class ButtonToggleOnOn : ButtonCommand
    {
        public ButtonToggleOnOn()
            : base("Toggles (On-On)", "text;Enter the first dx button in the toggle (1-127) and any options:")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            CommandInfoType CommandInfo = new CommandInfoType();
            CommandInfo = GameControlPlugin.GetCommandInfo(actionParameter);
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

            GameControlPlugin.buttons[CommandInfo.Value] = !GameControlPlugin.buttons[CommandInfo.Value];
            GameControlPlugin.buttons[CommandInfo.Value + 1] = !GameControlPlugin.buttons[CommandInfo.Value];
            GameControlPlugin.joystick.SetBtn(GameControlPlugin.buttons[CommandInfo.Value], GameControlPlugin.id, (uint)CommandInfo.Value);
            GameControlPlugin.joystick.SetBtn(GameControlPlugin.buttons[CommandInfo.Value + 1], GameControlPlugin.id, (uint)(CommandInfo.Value + 1));
            if (CommandInfo.DXSendType == 0)
            {
                if (GameControlPlugin.buttons[CommandInfo.Value])
                    Task.Delay(50).ContinueWith(t => GameControlPlugin.joystick.SetBtn(false, GameControlPlugin.id, (uint)CommandInfo.Value));
                else
                    Task.Delay(50).ContinueWith(t => GameControlPlugin.joystick.SetBtn(false, GameControlPlugin.id, (uint)(CommandInfo.Value + 1)));
            }

            this.ActionImageChanged(actionParameter);
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
                if (GameControlPlugin.buttons[commandInfo.Value])
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOnUpResourcePath : GameControlPlugin.ToggleUpResourcePath));
                else
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOnDownResourcePath : GameControlPlugin.ToggleDownResourcePath));
                if (commandInfo.DrawNumbers)
                    bitmapBuilder.DrawText(string.Format("{0}\n \n{1}", commandInfo.Value, commandInfo.Value + 1), -3, -5, 20, 80, fontSize: 10, lineHeight: 18);
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