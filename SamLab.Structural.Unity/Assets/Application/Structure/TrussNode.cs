using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assets.Application.Interfaces;
using SamLab.Structural.Core.Elements;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussNode : MonoBehaviour, IStructuralNode
    {
        public NodeData NodeData { get; set; }
        private TrussStructure _parentStructure;

        [SerializeField] public  List<TrussElement> ConnectedElements { get; private set; }
        public void Initialize(TrussStructure parentStructure)
        {
            _parentStructure = parentStructure;
            ConnectedElements = new List<TrussElement>();

        }

        private void OnMouseDown()
        {
            //_manager.OnNodeClicked(this);s
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

        private void OnMouseDrag()
        {
            //TODO : Do I really need to create a new Vector3 every frame?,
            //TODO:  I will need some transforms gizmos here.
            //TODO: spacemovement should be dependant of the direction of the drag.
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Let the structure's snap handler process this position
            Vector3 finalPosition = _parentStructure.SnapHandler.ProcessNodeDragPosition(this, worldPosition);

            // Apply the processed position
            transform.position = finalPosition;

        }

        private void OnMouseUp()
        {
            _parentStructure.SnapHandler.ProcessNodeRelease(this);
        }


    }
}