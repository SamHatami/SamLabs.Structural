using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Application.Interfaces;
using UnityEngine;

namespace Assets.Application
{
    public class CommandManager: MonoBehaviour
    {
        List<ICommand> Commands;


        private void Start()
        {
            Commands = new List<ICommand>();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            Commands.Add(new CreateNodeCommand());
            Commands.Add(new CreateMemberCommand());
        }

        void Update()
        {
            if (Input.GetKey("Q"))
            {
            }
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }

    internal class CreateMemberCommand : ICommand
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
