using Assets.Scripts.Core.Interfaces;
using System;

namespace Assets.Scripts.Structure.Commands
{
    public class ElementCommands
    {
        public class CreateElement : ICommand
        {
            public string Name { get; set; }

            public void Execute()
            {
                throw new NotImplementedException();
            }

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }
    }
}