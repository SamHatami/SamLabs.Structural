﻿using Structure.Base;
using Structure.Managers;
using Structure.Utils;
using UnityEngine;
using Workspace.Geometry.Interfaces;
using Workspace.Geometry.ReferenceGeometry;

namespace Workspace.Managers
{
    public class WorkspaceSnapHandler : MonoBehaviour //Should be composed as a sub-system for workspace
    {
        [SerializeField] private WorkspaceManager _workspaceManager;
        [SerializeField] public float NodeSnapTolerance { get; set; } = 0.1f;
        [SerializeField] private bool _enableNodeSnapping = true;
        [SerializeField] private bool _enableGridSnapping = false;
        [SerializeField] private bool _enableWorkPlaneSnapping = true; //this should be coming from workspace settings

        [SerializeField] private GameObject _gridSnapAdorner;
        [SerializeField] private GameObject _mergeNodeAdorner;
        private BasePlane _XYPlane { get; set; }
        private BasePlane _ZXPlane { get; set; }
        private BasePlane _YZPlane { get; set; }

        public bool EnableWorkPlaneSnapping
        {
            get => _enableWorkPlaneSnapping;
            set => _enableWorkPlaneSnapping = value;
        }

        [SerializeField] public float GridSize { get; set; } = 1f; //this should be coming from workspace settings
        [SerializeField] public IPlane ActiveWorkPlane { get; set; }
        [SerializeField] public WorkspaceSettings Settings { get; set; }

        private void Start()
        {
            _XYPlane = _workspaceManager.XYPlane.GetComponent<BasePlane>();
            _YZPlane = _workspaceManager.YZPlane.GetComponent<BasePlane>();
            _ZXPlane = _workspaceManager.XZPlane.GetComponent<BasePlane>();
            ActiveWorkPlane = _ZXPlane;
        }

        public void SetSnapSettings()
        {
        }

        public Vector3 ProcessNodeDragPosition(TrussNode node, Vector3 proposedPosition, TrussStructure parentStructure)
        {
            if (ActiveWorkPlane == null)
            {
                Debug.LogWarning("ActiveWorkPlane became null! Resetting to default...");

                // Reset to a default plane
                ActiveWorkPlane = _ZXPlane;

                // Optional: Log stack trace to help identify the cause
                Debug.LogWarning(System.Environment.StackTrace);
            }

            var finalPosition = proposedPosition;

            if (EnableWorkPlaneSnapping && ActiveWorkPlane != null)
                finalPosition = ProjectOntoWorkPlane(finalPosition);

            if (_enableGridSnapping)
            {
                var nearestGridNode = SnapToGrid(finalPosition);

                if (Vector3.Distance(nearestGridNode, finalPosition) < 0.25)
                {
                    _gridSnapAdorner.transform.position = nearestGridNode;
                    _gridSnapAdorner.SetActive(true);
                    finalPosition = nearestGridNode;
                }
                else
                {
                    _gridSnapAdorner.SetActive(false);
                }
            }

            if (!_enableNodeSnapping)
                return finalPosition;

            var nearestNode = NodeUtils.FindNearestNode(node, finalPosition, parentStructure, NodeSnapTolerance);


            if (nearestNode != null)
            {
                _mergeNodeAdorner.transform.position = nearestNode.transform.position;
                _mergeNodeAdorner.SetActive(true);
                return nearestNode.transform.position;
            }

            _mergeNodeAdorner.SetActive(false);


            return finalPosition;
        }

        private Vector3 ProjectOntoWorkPlane(Vector3 position)
        {
            if (ActiveWorkPlane == null)
                return position;

            var planePoint = ActiveWorkPlane.Origo;
            var planeNormal = ActiveWorkPlane.Normal;
            var v = position - planePoint;
            var distance = Vector3.Dot(v, planeNormal);
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
            _gridSnapAdorner.SetActive(false);
            _mergeNodeAdorner.SetActive(false);
            if (EnableWorkPlaneSnapping && ActiveWorkPlane != null)
            {
                var projectedPosition = ProjectOntoWorkPlane(releasedNode.transform.position);
                releasedNode.transform.position = projectedPosition;
            }

            if (!_enableNodeSnapping) return;

            var nearestNode = NodeUtils.FindNearestNode(releasedNode, releasedNode.transform.position, parentStructure,
                NodeSnapTolerance);

            if (nearestNode != null) NodeUtils.MergeNodes(nearestNode, releasedNode, parentStructure);
        }
    }
}