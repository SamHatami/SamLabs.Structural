using Structure.Base;
using Structure.Base.Constraints;
using Structure.Base.Loads;
using Structure.Managers;
using UnityEngine;

namespace Structure.Factories
{
    public class TrussFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _memberPrefab;
        [SerializeField] private GameObject _pinnnedSupportPrefab;
        [SerializeField] private GameObject _rollingSupportPrefab;
        [SerializeField] private GameObject _pointLoadPrefab;
        [SerializeField] private GameObject _trussStructure;

        public TrussStructure CreateStructure(TrussManager manager)
        {
            var structureObj = Instantiate(_trussStructure, Vector3.zero, Quaternion.identity);
            structureObj.transform.SetParent(manager.transform);

            //structureObj.name = "Structure_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            var structure = structureObj.GetComponent<TrussStructure>();
            if (structure == null)
                structure = structureObj.AddComponent<TrussStructure>();
            structure.Initialize(manager, this);
            return structure;
        }

        public TrussNode CreateNode(Vector3 position, TrussStructure parentStructure)
        {
            var nodeObj = Instantiate(_nodePrefab, position, Quaternion.identity);
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
            var memberObj = Instantiate(_memberPrefab, Vector3.zero, Quaternion.identity);
            //memberObj.name = "Member_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            memberObj.transform.SetParent(parentStructure.transform);
            var element = memberObj.GetComponent<TrussElement>();
            if (element == null)
                element = memberObj.AddComponent<TrussElement>();

            element.Initialize(parentStructure, startNode, endNode);
            return element;
        }

        public PointLoad CreatePointLoad(TrussNode node, Vector3 force = default)
        {
            var loadObj = Instantiate(_pointLoadPrefab, node.transform.position, Quaternion.identity);
            loadObj.transform.SetParent(node.ParentStructures[0].transform);

            var load = loadObj.GetComponent<PointLoad>();

            load.Force = force;

            return load;
        }

        public PinnedSupport CreatePinned(TrussNode node)
        {
            var pinnedObj = Instantiate(_pinnnedSupportPrefab, node.transform.position, Quaternion.identity);
            pinnedObj.transform.SetParent(node.ParentStructures[0].transform);
            var pinned = pinnedObj.GetComponent<PinnedSupport>();
            
            return pinned;
        }

        public RollingSupport CreateRollingSupport(TrussNode node)
        {
            var rollingObj = Instantiate(_pointLoadPrefab, node.transform.position, Quaternion.identity);
            rollingObj.transform.SetParent(node.ParentStructures[0].transform);
            var rolling = rollingObj.GetComponent<RollingSupport>();
            return rolling;
        }
    }
}