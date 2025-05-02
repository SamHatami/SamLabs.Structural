using System;
using System.Collections;
using Core.Application;
using Core.Interfaces;
using Structure.Base;
using Structure.Base.Loads;
using Structure.Factories;
using Structure.Managers;
using Workspace.Interaction;

namespace Structure.Commands
{
    public class AddPointLoad : ICoroutineCommand
    {
        private readonly TrussFactory _factory;
        private readonly SelectionInteraction _selection;
        public TrussNode Node { get; private set; }
        public PointLoad Load { get; private set; }
        public string Name { get; set; }

        public AddPointLoad(TrussManager trussManager, TrussFactory factory, SelectionInteraction selection)
        {
            _factory = factory;
            _selection = selection;
            //TODO: trussmanager.ActiveStructure.ActiveNodes.count > 1 => do selection other wise just create the load
            //Load = factory.CreatePointLoad(Node);
        }

        public void Execute()
        {
            CommandManager.Instance.ExecuteCommand(this);
        }

        private void OnNodePositionSet(TrussNode obj)
        {
            Node = obj;

            Load = _factory.CreatePointLoad(Node);
            Load.AttachedNode = Node;
            Node.ParentStructures[0].AddLoad(Load);
        }

        public void Undo()
        {
            Load.gameObject.SetActive(false);
            Load.AttachedNode = null;
            Node.ParentStructures[0].RemoveLoad(Load);
        }

        public IEnumerator ExecuteCoroutine()
        {
            if (Node == null)
            {
                _selection.gameObject.SetActive(true);
                _selection.Filter = SelectionFilterEnum.Node;
                _selection.NodeSelectionEvent += OnNodePositionSet;
                _selection.StartSelection();

                while (_selection.Selecting) yield return null;
            }

            if (Node == null)
                yield break;
        }
    }

    public class RemovePointLoad : ICommand
    {
        public PointLoad Load { get; }
        public string Name { get; set; }

        public RemovePointLoad(PointLoad load)
        {
            Load = load;
        }

        public void Execute()
        {
            Load.AttachedNode = null;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}