using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussFactory 
    {
        private GameObject _nodePrefab;
        private GameObject _memberPrefab;

        public TrussFactory()
        {
            // Load prefabs from Resources folder
            _nodePrefab = Resources.Load<GameObject>("Prefabs/Base/Skeletal/TrussNode");
            _memberPrefab = Resources.Load<GameObject>("Prefabs/Base/Skeletal/TrussMember");
        }

        public TrussNode CreateNode(Vector3 position, TrussManager manager)
        {
            GameObject nodeObj = GameObject.Instantiate(_nodePrefab, position, Quaternion.identity);
            //nodeObj.name = "Node_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            TrussNode node = nodeObj.GetComponent<TrussNode>();
            if (node == null)
                node = nodeObj.AddComponent<TrussNode>();

            node.Initialize(manager);
            return node;
        }

        public TrussMember CreateMember(TrussNode startNode, TrussNode endNode, TrussManager manager)
        {
            GameObject memberObj = GameObject.Instantiate(_memberPrefab, Vector3.zero, Quaternion.identity);
            //memberObj.name = "Member_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            TrussMember member = memberObj.GetComponent<TrussMember>();
            if (member == null)
                member = memberObj.AddComponent<TrussMember>();

            member.Initialize(manager, startNode, endNode);
            return member;
        }
    }
}
