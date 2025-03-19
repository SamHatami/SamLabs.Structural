using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Application.Structure;
using SamLab.Structural.Core.Interfaces;
using UnityEngine;

namespace Assets.Application.Workspace
{
    public class SnapHandler //Should be composed as a sub-system for workspace
    {
        //[SerializeField] private WorkspaceManager _workspaceManager;
        [SerializeField] public TrussStructure Structure { get; }

        [SerializeField] public float NodeSnapTolerance { get; set; } = 0.1f;
        [SerializeField] public bool EnableNodeSnapping { get; set; } = true;
        [SerializeField] public bool EnableGridSnapping { get; set; } = false;
        [SerializeField] public float GridSize { get; set; } = 1f; //this should be coming from workspace settings

        public SnapHandler(TrussStructure structure)
        {
            Structure = structure;
            //_workspaceManager = workspaceManager;
        }

        public void SetSnapSettings()
        {
        }

        public Vector3 ProcessNodeDragPosition(TrussNode node, Vector3 proposedPosition)
        {
            var finalPosition = proposedPosition;

            if (EnableGridSnapping) finalPosition = SnapToGrid(finalPosition);

            if (!EnableNodeSnapping) return finalPosition;

            var nearestNode = NodeUtils.FindNearestNode(node, finalPosition, Structure, NodeSnapTolerance);
            if (nearestNode != null) return nearestNode.transform.position;

            return finalPosition;
        }

        private Vector3 SnapToGrid(Vector3 position)
        {
            var x = Mathf.Round(position.x / GridSize) * GridSize;
            var y = Mathf.Round(position.y / GridSize) * GridSize;
            var z = Mathf.Round(position.z / GridSize) * GridSize;
            return new Vector3(x, y, z);
        }

        public void ProcessNodeRelease(TrussNode releasedNode)
        {
            if (!EnableNodeSnapping) return;

            var nearestNode = NodeUtils.FindNearestNode(releasedNode, releasedNode.transform.position, Structure,
                NodeSnapTolerance);
            if (nearestNode != null) NodeUtils.MergeNodes(nearestNode, releasedNode, Structure);
        }
    }
}