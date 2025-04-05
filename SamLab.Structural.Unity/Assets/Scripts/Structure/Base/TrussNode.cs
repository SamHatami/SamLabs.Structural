using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Managers;
using SamLab.Structural.Core.Elements;
using System.Collections.Generic;
using Assets.Scripts.Structure.Base.Loads;
using Assets.Scripts.Workspace.Geometry.ReferenceGeometry;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;
using MathNet.Numerics;

namespace Assets.Scripts.Structure.Base
{
    public class TrussNode : MonoBehaviour, IStructuralNode, ISelectable
    {
        [SerializeField] private List<TrussElement> _connectedElements;
        [SerializeField] private List<TrussStructure> _parentStructures;
        [SerializeField] private bool _isShared;
        [SerializeField] private bool _isMovable;
        [SerializeField] private bool _isSelected;
        [SerializeField] private PointLoad _load;
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
            Camera camera = Camera.main;

            // Create a ray from the camera through the mouse position
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // Define the plane for intersection
            Plane dragPlane;
            if (ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane != null)
            {
                // Use the active workspace plane
                Vector3 planeNormal = ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane.Normal;
                Vector3 planePoint = ParentStructures[0].WorkspaceSnapHandler.ActiveWorkPlane.Origo;
                dragPlane = new Plane(planeNormal, planePoint);
            }
            else
            {
                // Fallback: Use a plane perpendicular to the camera view
                dragPlane = new Plane(camera.transform.forward, transform.position);
            }

            // Check if the ray intersects the plane
            if (dragPlane.Raycast(ray, out float distance))
            {
                // Get the intersection point
                Vector3 hitPoint = ray.GetPoint(distance);

                // Process through snap handler for grid snapping, etc.
                Vector3 finalPosition = ParentStructures[0].WorkspaceSnapHandler
                    .ProcessNodeDragPosition(this, hitPoint, ParentStructures[0]);

                // Apply the position
                transform.position = finalPosition;
            }
        }

        public void OnMouseOver()
        {
            //Change color
        }

        public void OnMouseUp()
        {
            if (!IsMovable)
                return;

            if (_isSelected)
                ParentStructures[0].WorkspaceSnapHandler.ProcessNodeRelease(this, ParentStructures[0]);
            else
                _isSelected = true;
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