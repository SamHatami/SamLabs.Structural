using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Managers;
using SamLab.Structural.Core.Elements;
using System.Collections.Generic;
using Assets.Scripts.Structure.Base.Loads;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

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
            //TODO : Do I really need to create a new Vector3 every frame?,
            //TODO:  I will need some transforms gizmos here.
            //TODO: spacemovement should be dependant of the direction of the drag.

            //if(InteractionHandler.IsOccupied)
            //    return;

            if (!IsMovable)
                return;

            var mousePosition = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            var finalPosition = ParentStructures[0].WorkspaceSnapHandler
                .ProcessNodeDragPosition(this, worldPosition, ParentStructures[0]);

            transform.position = finalPosition;
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