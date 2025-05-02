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
        public event Action<TrussNode> NodeSelectionEvent;
        public event Action<TrussElement> ElementSelectionEvent;
        public event Action<PointLoad> LoadSelectionEvent;
        public event Action<TrussStructure> StructureSelectionEvent;
        public event Action<WorkPoint> WorkPointSelectionEvent;
        public event Action<WorkPlane> WorkPlaneSelectionEvent;


        public bool Selecting { get; set; }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && Selecting) // 0 = left mouse button
                CheckHits();
        }

        public void StartSelection()
        {
            Selecting = true;
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
                        NodeSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussNode>());
                        Selecting = false;
                        gameObject.SetActive(false);
                    }

                    break;
                case SelectionFilterEnum.Element:
                    if (hit.collider.gameObject.GetComponent<TrussElement>() != null)
                        ElementSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussElement>());
                    break;
                case SelectionFilterEnum.Load:
                    if (hit.collider.gameObject.GetComponent<PointLoad>() != null)
                        LoadSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<PointLoad>());
                    break;
                case SelectionFilterEnum.Structure:
                    if (hit.collider.gameObject.GetComponent<TrussStructure>() != null)
                        StructureSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussStructure>());
                    break;
                case SelectionFilterEnum.WorkPoint:
                    if (hit.collider.gameObject.GetComponent<WorkPoint>() != null)
                    {
                        WorkPointSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<WorkPoint>());
                        Selecting = false;
                        gameObject.SetActive(false);
                    }

                    break;
                case SelectionFilterEnum.WorkPlane:
                    if (hit.collider.gameObject.GetComponent<WorkPlane>() != null)
                    {
                        WorkPlaneSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<WorkPlane>());
                        Selecting = false;
                        gameObject.SetActive(false);
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