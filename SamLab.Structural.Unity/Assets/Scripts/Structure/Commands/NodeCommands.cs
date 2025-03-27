using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Managers;
using UnityEngine;
namespace Assets.Scripts.Structure.Commands
{
    public class CreateNodeCommand : MonoBehaviour,ICommand
    {
        public Vector3 Position { get; }
        private readonly TrussManager _trussManager;
        private TrussNode _node;
        public string Name { get; set; } = "Create Node";

        public CreateNodeCommand(TrussManager trussManager, Vector3 position = default)
        {
            Position = position;
            _trussManager = trussManager;
        }

        public void Execute()
        {
           _node = _trussManager.ActiveStructure.CreateNode(Position);
        }

        public void Undo()
        {
            _trussManager.ActiveStructure.RemoveNode(_node);
        }
    }

    public class RemoveNodeCommand : MonoBehaviour, ICommand
    {
        public Vector3 Position { get; }
        private readonly TrussManager _trussManager;
        private TrussNode _node;
        public string Name { get; set; } = "Create Node";

        public RemoveNodeCommand(TrussNode node)
        {
            _node = node;
        }

        public void Execute()
        {
            if(_node.IsShared)
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


    public class SetNodePositionCommand : MonoBehaviour, ICommand
    {
        private Vector3 _newPosition { get; }
        private TrussNode _node;
        private readonly Vector3 _oldPosition;
        public string Name { get; set; } = "Create Node";

        public SetNodePositionCommand(TrussNode node, Vector3 newPosition)
        {
            _node = node;
            _oldPosition = _node.transform.position;
        }

        public void Execute()
        {
            //Can move shared node if move by positionCommand

            _node.transform.position = new Vector3(_newPosition.x,_newPosition.y,_newPosition.z);

            //if need positions is one another node ask if user wants to merge nodes
            //TOOD: Implement merge nodes
        }

        public void Undo()
        {
            _node.transform.position = _oldPosition;
        }
    }

    public class MergeNodesCommand : MonoBehaviour, ICommand
    {
        public string Name { get; set; }

        public MergeNodesCommand()
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
