using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussFactory
    {
        private GameObject _nodePrefab;
        private GameObject _memberPrefab;
        private GameObject _supportPrefab;
        private GameObject _trussStructure;

        public TrussFactory()
        {
            // Load prefabs from Resources folder
            _trussStructure = Resources.Load<GameObject>("Prefabs/Base/TrussStructure");
            _nodePrefab = Resources.Load<GameObject>("Prefabs/Base/Skeletal/TrussNode");
            _memberPrefab = Resources.Load<GameObject>("Prefabs/Base/Skeletal/TrussElement");
        }

        public TrussStructure CreateStructure(TrussManager manager)
        {
            var structureObj =  GameObject.Instantiate(_trussStructure, Vector3.zero, Quaternion.identity);
            structureObj.transform.SetParent(manager.transform);

            //structureObj.name = "Structure_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            var structure = structureObj.GetComponent<TrussStructure>();
            if (structure == null)
                structure = structureObj.AddComponent<TrussStructure>();
            structure.Initialize(manager);
            return structure;
        }
        public TrussNode CreateNode(Vector3 position, TrussStructure parentStructure)
        {
            var nodeObj = GameObject.Instantiate(_nodePrefab, position, Quaternion.identity);
            //nodeObj.name = "Node_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            nodeObj.transform.SetParent(parentStructure.transform);
            var node = nodeObj.GetComponent<TrussNode>();
            if (node == null)
                node = nodeObj.AddComponent<TrussNode>();

            node.Initialize(parentStructure);
            return node;
        }

        public TrussElement CreateMember(TrussNode startNode, TrussNode endNode, TrussStructure parentStructure)
        {
            var memberObj = GameObject.Instantiate(_memberPrefab, Vector3.zero, Quaternion.identity);
            //memberObj.name = "Member_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            memberObj.transform.SetParent(parentStructure.transform);
            var element = memberObj.GetComponent<TrussElement>();
            if (element == null)
                element = memberObj.AddComponent<TrussElement>();

            element.Initialize(parentStructure, startNode, endNode);
            return element;
        }
    }
}