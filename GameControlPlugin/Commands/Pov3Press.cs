namespace Loupedeck.GameControlPlugin.Commands
{
    internal class Pov3Press : PovCommand
    {
        public Pov3Press()
            : base("POV1 Press", "text;Enter the direction to press (Up, Down, Left, Right) and any options:")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            DoCommand(3U, actionParameter);
        }
    }
}