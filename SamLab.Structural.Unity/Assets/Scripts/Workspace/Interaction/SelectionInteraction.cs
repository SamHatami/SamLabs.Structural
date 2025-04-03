using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Base.Loads;
using Assets.Scripts.Structure.Managers;
using UnityEngine;

namespace Assets.Scripts.Workspace.Interaction
{
    public class SelectionInteraction : MonoBehaviour
    {
        public SelectionFilterEnum Filter;
        public event Action<TrussNode> NodeSelectionEvent;
        public event Action<TrussElement> ElementSelectionEvent;
        public event Action<PointLoad> LoadSelectionEvent;
        public event Action<TrussStructure> StructureSelectionEvent;

        public bool Selecting { get; set; }

        void Update()
        {

            if (Input.GetMouseButtonUp(0) && Selecting) // 0 = left mouse button
            {
                CheckHits();
            }


        }

        public void StartSelection()
        {
            Selecting = true;
        }

        private void CheckHits()
        {
            RaycastHit hit;

            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit)) return;
            switch (Filter)
            {
                case SelectionFilterEnum.Node:
                    if (hit.collider.gameObject.GetComponent<TrussNode>() != null)
                    {
                        //invoke NodeSelectionEvent and send back the node that was clicked  
                        NodeSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussNode>());
                        Selecting = false;
                        this.gameObject.SetActive(false);
                    }
                    break;
                case SelectionFilterEnum.Element:
                    if (hit.collider.gameObject.GetComponent<TrussElement>() != null)
                    {
                        ElementSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussElement>());
                    }
                    break;
                case SelectionFilterEnum.Load:
                    if (hit.collider.gameObject.GetComponent<PointLoad>() != null)
                    {
                        LoadSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<PointLoad>());
                    }
                    break;
                case SelectionFilterEnum.Structure:
                    if (hit.collider.gameObject.GetComponent<TrussStructure>() != null)
                    {
                        StructureSelectionEvent?.Invoke(hit.collider.gameObject.GetComponent<TrussStructure>());
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
        Structure
    }
}
