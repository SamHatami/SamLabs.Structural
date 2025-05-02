using Core.Interfaces;
using UnityEngine;

namespace Workspace.Geometry.ReferenceGeometry
{
    public class WorkPoint : MonoBehaviour, ISelectable
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public bool IsMovable { get; set; } = false;

        public void OnMouseDown()
        {
        }

        public void OnMouseUp()
        {
        }

        public void OnMouseDrag()
        {
        }

        public void OnMouseOver()
        {
        }
    }
}