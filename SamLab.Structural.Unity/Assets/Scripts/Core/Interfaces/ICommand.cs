namespace Assets.Scripts.Core.Interfaces
{
    public interface ICommand
    {
        public string Name { get; set; }
        public void Execute();
        public void Undo();
    }
}