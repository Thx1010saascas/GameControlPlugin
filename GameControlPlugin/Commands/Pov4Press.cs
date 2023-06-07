namespace Loupedeck.GameControlPlugin.Commands
{
    internal class Pov4Press : PovCommand
    {
        public Pov4Press()
            : base("POV1 Press", "text;Enter the direction to press (Up, Down, Left, Right) and any options:")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            DoCommand(4U, actionParameter);
        }
    }
}