using Core.Interfaces;
using Structure.Base;
using Structure.Managers;
using UnityEngine;
using Workspace.Managers;

namespace Structure.Commands
{
    public class CreateNode : ICommand
    {
        public Vector3 Position { get; }
        private readonly TrussManager _trussManager;
        private readonly InteractionHandler _interactions;
        private TrussNode _node;
        public string Name { get; set; } = "Create Node";

        public CreateNode(TrussManager trussManager, InteractionHandler interactions)
        {
            _trussManager = trussManager;
            _interactions = interactions;
        }

        public void Execute()
        {
            // subscribe to node placement event
            //Wait on the event to get the position

            _node = _trussManager.ActiveStructure.CreateNode(Position);
        }

        public void Undo()
        {
            _trussManager.ActiveStructure.RemoveNode(_node);
        }
    }

    public class RemoveNode : ICommand
    {
        public Vector3 Position { get; }
        private readonly TrussManager _trussManager;
        private TrussNode _node;
        public string Name { get; set; } = "Create Node";

        public RemoveNode(TrussNode node)
        {
            _node = node;
        }

        public void Execute()
        {
            if (_node.IsShared)
                return; //ask if user wants to remove the shared node or remove specific members instead

            _node.ParentStructures[0].RemoveNode(_node);
            _node.gameObject.SetActive(false);
        }

        public void Undo()
        {
            _node.gameObject.SetActive(true);
            _trussManager.ActiveStructure.AddNode(_node);
        }
    }

    public class SetNodePosition : ICommand
    {
        private Vector3 _newPosition { get; }
        private TrussNode _node;
        private readonly Vector3 _oldPosition;
        public string Name { get; set; } = "Create Node";

        public SetNodePosition(TrussNode node, Vector3 newPosition)
        {
            //Select node and get new position from event
            _node = node;
            _oldPosition = _node.transform.position;
        }

        public void Execute()
        {
            //Can move shared node if move by positionCommand

            _node.transform.position = new Vector3(_newPosition.x, _newPosition.y, _newPosition.z);

            //if need positions is one another node ask if user wants to merge nodes
            //TOOD: Implement merge nodes
        }

        public void Undo()
        {
            _node.transform.position = _oldPosition;
        }
    }

    public class MergeNodes : ICommand
    {
        public string Name { get; set; }

        public MergeNodes()
        {
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}