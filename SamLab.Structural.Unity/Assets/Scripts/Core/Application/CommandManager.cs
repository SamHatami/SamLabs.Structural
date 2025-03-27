using System;
using System.Collections.Generic;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Commands;
using Assets.Scripts.Structure.Managers;
using UnityEngine;

namespace Assets.Scripts.Core.Application
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
            Commands.Add(new CreateNodeCommand(FindFirstObjectByType<TrussManager>()));
            Commands.Add(new CreateMemberCommand());
        }

        void Update()
        {

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
