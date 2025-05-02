using Core.Interfaces;

namespace Workspace.Commands
{
    public class SomeNewCommand : ICommand
    {
        public string Name { get; set; }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}