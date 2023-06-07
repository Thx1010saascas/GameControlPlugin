namespace Loupedeck.GameControlPlugin.Commands
{
    using System;

    internal class ButtonOff : ButtonCommand
    {
        public ButtonOff()
            : base("Button Off", "text;Enter the dx button to turn off (1-128) and any options:")
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

            GameControlPlugin.joystick.SetBtn(false, GameControlPlugin.id, (uint)commandInfo.Value);
            this.ActionImageChanged(actionParameter);
        }
    }
}