using Assets.Scripts.Core.Interfaces;

namespace Assets.Scripts.Workspace.Commands
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