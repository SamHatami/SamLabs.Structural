using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Managers;
using Assets.Scripts.Structure.Utils;
using Assets.Scripts.Workspace.Geometry.ReferenceGeometry;
using UnityEngine;

namespace Assets.Scripts.Workspace.Managers
{
    public class WorkspaceSnapHandler : MonoBehaviour //Should be composed as a sub-system for workspace
    {
        [SerializeField] private WorkspaceManager _workspaceManager;
        [SerializeField] public float NodeSnapTolerance { get; set; } = 0.1f;
        [SerializeField] public bool EnableNodeSnapping { get; set; } = true;
        [SerializeField] public bool EnableGridSnapping { get; set; } = true;
        [SerializeField] private bool _enableWorkPlaneSnapping = true; //this should be coming from workspace settings

        public bool EnableWorkPlaneSnapping
        {
            get => _enableWorkPlaneSnapping;
            set => _enableWorkPlaneSnapping = value;
        } 
        [SerializeField] public float GridSize { get; set; } = 1f; //this should be coming from workspace settings
        [SerializeField] public WorkPlane ActiveWorkPlane { get; set; }
        [SerializeField] public WorkspaceSettings Settings { get; set; }

        public void SetSnapSettings()
        {
        }

        public Vector3 ProcessNodeDragPosition(TrussNode node, Vector3 proposedPosition, TrussStructure parentStructure)
        {
            var finalPosition = proposedPosition;

            if (EnableWorkPlaneSnapping && ActiveWorkPlane != null)
                finalPosition = ProjectOntoWorkPlane(finalPosition);

            if (EnableGridSnapping) finalPosition = SnapToGrid(finalPosition);

            if (!EnableNodeSnapping)
                return finalPosition;

            var nearestNode = NodeUtils.FindNearestNode(node, finalPosition, parentStructure, NodeSnapTolerance);
            if (nearestNode != null) return nearestNode.transform.position;

            return finalPosition;
        }

        private Vector3 ProjectOntoWorkPlane(Vector3 position)
        {
            if (ActiveWorkPlane == null)
                return position;

            Vector3 planePoint = ActiveWorkPlane.transform.position;
            Vector3 planeNormal = ActiveWorkPlane.Normal;
            Vector3 v = position - planePoint;
            float distance = Vector3.Dot(v, planeNormal);
            return position - distance * planeNormal;
        }

        private Vector3 SnapToGrid(Vector3 position) //move to utils?
        {
            var x = Mathf.Round(position.x / GridSize) * GridSize;
            var y = Mathf.Round(position.y / GridSize) * GridSize;
            var z = Mathf.Round(position.z / GridSize) * GridSize;
            return new Vector3(x, y, z);
        }

        public void ProcessNodeRelease(TrussNode releasedNode, TrussStructure parentStructure)
        {
            if (EnableWorkPlaneSnapping && ActiveWorkPlane != null)
            {
                Vector3 projectedPosition = ProjectOntoWorkPlane(releasedNode.transform.position);
                releasedNode.transform.position = projectedPosition;
            }

            if (!EnableNodeSnapping) return;

            var nearestNode = NodeUtils.FindNearestNode(releasedNode, releasedNode.transform.position, parentStructure,
                NodeSnapTolerance);

            if (nearestNode != null) NodeUtils.MergeNodes(nearestNode, releasedNode, parentStructure);
        }
    }
}