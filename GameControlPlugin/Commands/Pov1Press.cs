namespace Loupedeck.GameControlPlugin.Commands
{
    internal class Pov1Press : PovCommand
    {
        public Pov1Press()
            : base("POV1 Press", "text;Enter the direction to press (Up, Down, Left, Right) and any options:")
        {
        }

        protected override void RunCommand(string actionParameter)
        {
            DoCommand(1U, actionParameter);
        }
    }
}