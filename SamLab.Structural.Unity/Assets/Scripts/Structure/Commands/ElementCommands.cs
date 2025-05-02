using System;
using Core.Interfaces;

namespace Structure.Commands
{
    public class ElementCommands
    {
        public class CreateElement : ICommand
        {
            public string Name { get; set; }

            public void Execute()
            {
            }

            public void Undo()
            {
            }
        }
    }
}