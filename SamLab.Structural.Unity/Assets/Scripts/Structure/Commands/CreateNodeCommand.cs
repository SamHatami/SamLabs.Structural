using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Structure.Commands
{
    public class CreateNodeCommand : ICommand
    {
        public string Name { get; set; } = "Create Node";

        public void Execute()
        {

        }

        public void Undo()
        {
        }
    }
}
