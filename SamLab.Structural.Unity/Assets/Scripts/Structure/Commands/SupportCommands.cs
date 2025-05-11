using System.Collections;
using Core.Application;
using Core.Interfaces;
using Structure.Base;
using Structure.Base.Constraints;
using Structure.Factories;
using Structure.Managers;
using Workspace.Interaction;
using Workspace.Managers;

namespace Structure.Commands
{

    public class AddPinnedSupport : ICoroutineCommand
    {
        private readonly TrussManager _trussManager;
        private readonly TrussFactory _factory;
        private readonly SelectionInteraction _selection;
        public TrussNode Node { get; private set; }
        public PinnedSupport Pinned { get; private set; }
        public string Name { get; set; }

        public AddPinnedSupport(TrussManager trussManager, TrussFactory factory, SelectionInteraction interactions)
        {
            _trussManager = trussManager;
            _factory = factory;
            _selection = interactions;
        }

        public void Execute()
        {
            CommandManager.Instance.ExecuteCommand(this);
        }
        
        public IEnumerator ExecuteCoroutine()
        {
            if (Node == null)
            {
                _selection.gameObject.SetActive(true);
                _selection.Filter = SelectionFilterEnum.Node;
                SelectionEvents.NodeSelectedEvent += OnNodePositionSet;
                _selection.StartSelection();

                while (_selection.Selecting) yield return null;
            }

            if (Node == null)
                yield break;
        }

        private void OnNodePositionSet(TrussNode obj)
        {
            Node = obj;

            Pinned = _factory.CreatePinned(Node);
            Pinned.AttachedNode = Node;
            Node.ParentStructures[0].AddSupport(Pinned);
        }

        public void Undo()
        {
            Node.ParentStructures[0].RemoveSupport(Pinned);
        }
    }
}