﻿namespace Loupedeck.GameControlPlugin.Commands
{
    internal class Pov2Press : PovCommand
    {
        public Pov2Press()
            : base("POV1 Press", "text;Enter the direction to press (Up, Down, Left, Right) and any options:")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            DoCommand(2U, actionParameter);
        }
    }
}