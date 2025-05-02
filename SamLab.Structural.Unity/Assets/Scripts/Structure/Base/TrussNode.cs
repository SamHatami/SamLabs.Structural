using System;
using System.Collections.Generic;
using Core.Interfaces;
using SamLab.Structural.Core.Elements;
using Structure.Base.Loads;
using Structure.Managers;
using UnityEngine;

namespace Structure.Base
{
    public class TrussNode : MonoBehaviour, IStructuralNode, ISelectable
    {
        [SerializeField] private List<TrussElement> _connectedElements;
        [SerializeField] private List<TrussStructure> _parentStructures;
        [SerializeField] private bool _isShared;
        [SerializeField] private bool _isMovable;
        [SerializeField] private bool _isSelected;
        [SerializeField] private PointLoad _load;
        [SerializeField] private Transform _transform;
        [SerializeField] private NodeData _nodeData;


        public bool IsShared
        {
            get => _isShared;
            set
            {
                _isShared = value;
                IsMovable = !value;
            }
        }

        public bool IsMovable
        {
            get => _isMovable;
            set
            {
                if (IsShared)
                    return;

                _isMovable = value;
            }
        }

        public NodeData NodeData { get; set; }

        private void Awake()
        {
            _transform = transform;
            if(_nodeData == null)
                _nodeData = new NodeData();
        }


        public List<TrussStructure> ParentStructures
        {
            get => _parentStructures;
            private set => _parentStructures = value;
        }

        public List<TrussElement> ConnectedElements
        {
            get => _connectedElements;
            private set => _connectedElements = value;
        }

        public void Initialize(TrussStructure parentStructure)
        {
            ParentStructures = new List<TrussStructure> { parentStructure };
            ConnectedElements = new List<TrussElement>();
            IsMovable = true;
        }


        public void OnMouseDown()
        {
            //Change color to selected or outline
            //add this to current selection
        }

        public void RemoveConnectedElement(TrussElement element)
        {
            if (ConnectedElements.Contains(element))
                ConnectedElements.Remove(element);
        }

        public void AddConnectedElement(TrussElement element)
        {
            if (!ConnectedElements.Contains(element))
                ConnectedElements.Add(element);
        }

        public void OnMouseDrag()
        {
            if (!IsMovable)
                return;

            // Get the current camera
            var camera = Camera.main;

            // Create a ray from the camera through the mouse position
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            // Define the plane for intersection
            Plane dragPlane;
            if (ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane != null)
            {
                // Use the active workspace plane
                var planeNormal = ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane.Normal;
                var planePoint = ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane.Origo;
                dragPlane = new Plane(planeNormal, planePoint);
            }
            else
            {
                // Fallback: Use a plane perpendicular to the camera view
                dragPlane = new Plane(camera.transform.forward, transform.position);
            }

            // Check if the ray intersects the plane
            if (dragPlane.Raycast(ray, out var distance))
            {
                // Get the intersection point
                var hitPoint = ray.GetPoint(distance);

                // Process through snap handler for grid snapping, etc.
                var finalPosition = ParentStructures[0].WorkspaceSnapHandler
                    .ProcessNodeDragPosition(this, hitPoint, ParentStructures[0]);

                // Apply the position
                transform.position = finalPosition;

                UpdateData();
            }
        }

        private void UpdateData()
        {
            _nodeData.Position = _transform.position;
        }

        public void OnMouseOver()
        {
            //Change color
        }

        public void OnMouseUp()
        {
            if (!IsMovable)
                return;

            ParentStructures[0].WorkspaceSnapHandler.ProcessNodeRelease(this, ParentStructures[0]);
        }

        public void AddParentStructure(TrussStructure structure)
        {
            if (ParentStructures.Contains(structure))
                return;
            ParentStructures.Add(structure);

            if (ParentStructures.Count > 1) IsShared = true;
            //Change material to shared node material
        }

        public void RemoveParentStructure(TrussStructure structure)
        {
            if (!ParentStructures.Contains(structure))
                return;
            ParentStructures.Remove(structure);

            if (ParentStructures.Count <= 1) IsShared = false;
            //Change material to normal node material
        }
    }
}