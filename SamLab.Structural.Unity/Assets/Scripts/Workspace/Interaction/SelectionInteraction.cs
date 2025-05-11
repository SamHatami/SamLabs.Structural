using System;
using Structure.Base;
using Structure.Base.Loads;
using Structure.Managers;
using UnityEngine;
using Workspace.Geometry.ReferenceGeometry;

namespace Workspace.Interaction
{
    public class SelectionInteraction : MonoBehaviour
    {
        public SelectionFilterEnum Filter;
        public bool MultiSelect;


        public bool Selecting { get; set; }

        private void Update()
        {
            if (!Input.GetButtonDown("Fire1")) return;

            if (Selecting)
                CheckHits();
            else
                CheckUnfiltered();
        }

        public void StartSelection()
        {
            Selecting = true;
        }

        public void SelectNodeAndPublish(TrussNode node)
        {
            SelectionEvents.PublishNodeSelected(node);
        }
        private void CheckUnfiltered()
        {
            RaycastHit hit;
            
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit)) return;

            if (hit.collider.gameObject.GetComponent<TrussNode>() != null)
            {
                
                SelectionEvents.PublishNodeSelected(hit.collider.gameObject.GetComponent<TrussNode>());
                Selecting = false;
            }

            if (hit.collider.gameObject.GetComponent<TrussMember>() != null)
                SelectionEvents.PublishMemberSelected(hit.collider.gameObject.GetComponent<TrussMember>());

            if (hit.collider.gameObject.GetComponent<PointLoad>() != null)
                SelectionEvents.PublishLoadSelected(hit.collider.gameObject.GetComponent<PointLoad>());

            if (hit.collider.gameObject.GetComponent<TrussStructure>() != null)
                SelectionEvents.PublishStructureSelected(hit.collider.gameObject.GetComponent<TrussStructure>());

            if (hit.collider.gameObject.GetComponent<WorkPoint>() != null)
            {
                SelectionEvents.PublishWorkPointSelected(hit.collider.gameObject.GetComponent<WorkPoint>());
                Selecting = false;
            }

            if (hit.collider.gameObject.GetComponent<WorkPlane>() != null)
            {
                SelectionEvents.PublishWorkPlaneSelectedEvent(hit.collider.gameObject.GetComponent<WorkPlane>());
                Selecting = false;
            }
        }

        private void CheckHits()
        {
            RaycastHit hit;

            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit)) return;
            switch (Filter)
            {
                case SelectionFilterEnum.Node:
                    if (hit.collider.gameObject.GetComponent<TrussNode>() != null)
                    {
                        SelectionEvents.PublishNodeSelected(hit.collider.gameObject.GetComponent<TrussNode>());
                        Selecting = false;
                    }

                    break;
                case SelectionFilterEnum.Element:
                    if (hit.collider.gameObject.GetComponent<TrussMember>() != null)
                        SelectionEvents.PublishMemberSelected(hit.collider.gameObject.GetComponent<TrussMember>());
                    break;
                case SelectionFilterEnum.Load:
                    if (hit.collider.gameObject.GetComponent<PointLoad>() != null)
                        SelectionEvents.PublishLoadSelected(hit.collider.gameObject.GetComponent<PointLoad>());
                    break;
                case SelectionFilterEnum.Structure:
                    if (hit.collider.gameObject.GetComponent<TrussStructure>() != null)
                        SelectionEvents.PublishStructureSelected(hit.collider.gameObject.GetComponent<TrussStructure>());
                    break;
                case SelectionFilterEnum.WorkPoint:
                    if (hit.collider.gameObject.GetComponent<WorkPoint>() != null)
                    {
                        SelectionEvents.PublishWorkPointSelected(hit.collider.gameObject.GetComponent<WorkPoint>());
                        Selecting = false;
                    }

                    break;
                case SelectionFilterEnum.WorkPlane:
                    if (hit.collider.gameObject.GetComponent<WorkPlane>() != null)
                    {
                        SelectionEvents.PublishWorkPlaneSelectedEvent(hit.collider.gameObject.GetComponent<WorkPlane>());
                        Selecting = false;
                    }

                    break;
                default:
                    break;
            }
        }
    }

    public enum SelectionFilterEnum
    {
        Node,
        Element,
        Load,
        Structure,
        WorkPoint,
        WorkPlane
    }
}