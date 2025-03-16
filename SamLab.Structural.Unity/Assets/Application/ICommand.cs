namespace Assets.Application
{
    public interface ICommand
    {
        public string Name { get; set; }
        public void Execute();
        public void Undo();
    }
}