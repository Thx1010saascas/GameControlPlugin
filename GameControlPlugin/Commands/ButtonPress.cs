﻿namespace Loupedeck.GameControlPlugin.Commands
{
    using System.Threading.Tasks;

    internal class ButtonPress : ButtonCommand
    {
        public ButtonPress()
            : base("Button Press", "text;Enter the dx button to press (1-128) and any options:")
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

            Joystick joystick = JoystickManager.GetJoystick(actionParameter);
            
            joystick.SetBtn(true, (uint)CommandInfo.Value);
            
            Task.Delay(JoystickManager.ButtonPressDelay).ContinueWith(t => joystick.SetBtn(false, (uint)CommandInfo.Value));
            
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
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(commandInfo.ButtonPath));
                if (commandInfo.DrawNumbers)
                    bitmapBuilder.DrawText($"{commandInfo.Value}", fontSize: 20);
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