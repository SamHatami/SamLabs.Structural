using Assets.Application;
using Assets.Application.Interfaces;

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