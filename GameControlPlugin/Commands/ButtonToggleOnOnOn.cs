namespace Loupedeck.GameControlPlugin.Commands
{
    using System.Threading.Tasks;

    internal class ButtonToggleOnOnOn : ButtonCommand
    {
        public ButtonToggleOnOnOn()
            : base("Toggles (On-On-On)", "text;Enter the first dx button in the toggle (1-125) and any options:")
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

            if (GameControlPlugin.buttons[CommandInfo.Value])
            {
                GameControlPlugin.buttons[CommandInfo.Value] = false;
                GameControlPlugin.buttons[CommandInfo.Value + 1] = true;
                GameControlPlugin.buttons[CommandInfo.Value + 2] = false;
            }
            else if (GameControlPlugin.buttons[CommandInfo.Value + 1])
            {
                GameControlPlugin.buttons[CommandInfo.Value] = false;
                GameControlPlugin.buttons[CommandInfo.Value + 1] = false;
                GameControlPlugin.buttons[CommandInfo.Value + 2] = true;
            }
            else
            {
                GameControlPlugin.buttons[CommandInfo.Value] = true;
                GameControlPlugin.buttons[CommandInfo.Value + 1] = false;
                GameControlPlugin.buttons[CommandInfo.Value + 2] = false;
            }

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);

            joystick.SetBtn(GameControlPlugin.buttons[CommandInfo.Value], (uint)CommandInfo.Value);
            joystick.SetBtn(GameControlPlugin.buttons[CommandInfo.Value + 1], (uint)(CommandInfo.Value + 1));
            joystick.SetBtn(GameControlPlugin.buttons[CommandInfo.Value + 2], (uint)(CommandInfo.Value + 2));
            
            if (CommandInfo.DXSendType == 0)
            {
                if (GameControlPlugin.buttons[CommandInfo.Value])
                    Task.Delay(JoystickManager.ButtonPressDelay).ContinueWith(t => joystick.SetBtn(false, (uint)CommandInfo.Value));
                else if (GameControlPlugin.buttons[CommandInfo.Value + 1])
                    Task.Delay(JoystickManager.ButtonPressDelay).ContinueWith(t => joystick.SetBtn(false, (uint)(CommandInfo.Value + 1)));
                else
                    Task.Delay(JoystickManager.ButtonPressDelay).ContinueWith(t => joystick.SetBtn(false, (uint)(CommandInfo.Value + 2)));
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
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOnOnUpResourcePath : GameControlPlugin.ToggleUpResourcePath));
                else if (GameControlPlugin.buttons[commandInfo.Value + 1])
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOnOnMiddleResourcePath : GameControlPlugin.ToggleMiddleResourcePath));
                else
                    bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.DrawToggleIndicators ? GameControlPlugin.ToggleOnOnOnDownResourcePath : GameControlPlugin.ToggleDownResourcePath));
                if (commandInfo.DrawNumbers)
                    bitmapBuilder.DrawText(string.Format("{0}\n{1}\n{2}", commandInfo.Value, commandInfo.Value + 1, commandInfo.Value + 2), -3, -5, 20, 80, fontSize: 10, lineHeight: 18);
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