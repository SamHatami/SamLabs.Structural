using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Commands;
using Assets.Scripts.Structure.Managers;
using System.Collections.Generic;
using Assets.Scripts.Structure.Factories;
using Assets.Scripts.Workspace.Interaction;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

namespace Assets.Scripts.Core.Application
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }

        private List<ICommand> _commands;
        private Stack<ICommand> _commandHistory;
        private TrussManager _trussManager;
        private InteractionHandler _interactionHandler;
        private TrussFactory _trussFactory;
        public SelectionInteraction _SelectionInteraction;


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            _commands = new List<ICommand>();
            _commandHistory = new Stack<ICommand>();
            _trussManager = FindFirstObjectByType<TrussManager>();
            _interactionHandler = FindFirstObjectByType<InteractionHandler>();
            _trussFactory = FindFirstObjectByType<TrussFactory>();
            _SelectionInteraction = FindFirstObjectByType<SelectionInteraction>();
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            //if we have a lot of commands, we can use reflection to automatically register them?
       
            _commands.Add(new CreateNode(_trussManager,_interactionHandler));
            _commands.Add(new ElementCommands.CreateElement());
            _commands.Add(new AddPointLoad(_trussManager, _trussFactory, _SelectionInteraction));
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.S))
            {
                var addLoad = new AddPointLoad(_trussManager, _trussFactory, _SelectionInteraction);
                ExecuteCommand(addLoad);
            }

            //Handle keys inputs for commands here?
            //Dictionary that holds input keys and commands?
        }

        public void CancelActiveCommand()
        {

        }

        public void ExecuteCommand(ICommand command)
        {

            if (command is ICoroutineCommand coroutineCommand)
            {
                StartCoroutine(coroutineCommand.ExecuteCoroutine());
            }
            else
            {
                command.Execute();
            }

            _commandHistory.Push(command);
        }

        public void UndoCommand()
        {
            _commandHistory.Pop();
        }
    }
}