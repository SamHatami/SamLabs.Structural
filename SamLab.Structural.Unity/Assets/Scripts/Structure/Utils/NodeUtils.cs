using System.Linq;
using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Managers;
using UnityEngine;

namespace Assets.Scripts.Structure.Utils
{
    public static class NodeUtils
    {
        public static bool CanMergeNodes(TrussNode targetNode, TrussNode nodeToMerge)
        {
            foreach (var element in targetNode.ConnectedElements)
            {
                if (element.StartNode == nodeToMerge || element.EndNode == nodeToMerge)
                    return false;
            
            }

            foreach (var element1 in nodeToMerge.ConnectedElements)
            {
                TrussNode otherNode1 = element1.StartNode == nodeToMerge ? element1.EndNode : element1.StartNode;

                foreach (var element2 in targetNode.ConnectedElements)
                {
                    TrussNode otherNode2 = element2.StartNode == targetNode ? element2.EndNode : element2.StartNode;

                    if (otherNode1 == otherNode2)
                        return false;
                
                }
            }

            return true;
        }

        public static void MergeNodes(TrussNode targetNode, TrussNode nodeToMerge, TrussStructure structure)
        {
            if (!CanMergeNodes(targetNode, nodeToMerge))
                return;

            foreach (var element in nodeToMerge.ConnectedElements.ToList())
            {
                if (element.StartNode == nodeToMerge)
                    element.SetStartNode(targetNode);

                if (element.EndNode == nodeToMerge)
                    element.SetEndNode(targetNode);
            }

            nodeToMerge.ConnectedElements.Clear();

            structure.DeleteNode(nodeToMerge);
        }

        public static TrussNode FindNearestNode(TrussNode node, Vector3 proposedPosition, TrussStructure structure, float tolerance)
        {
            TrussNode nearestNode = null;
            float minDistance = tolerance;

            foreach (var otherNode in structure.Nodes)
            {
                if (otherNode == node) continue;

                float distance = Vector3.Distance(otherNode.transform.position, proposedPosition);
                if (!(distance < minDistance)) continue;

                if (!CanMergeNodes(otherNode, node)) continue;

                minDistance = distance;
                nearestNode = otherNode;
            }

            return nearestNode;
        }
    }
}