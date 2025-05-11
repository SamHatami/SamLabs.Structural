using System.Collections.Generic;
using Core.Interfaces;
using Structure.Base.Loads;
using Workspace.Interaction;
using Structure.Base;
using Structure.Managers;
using Workspace.Geometry.ReferenceGeometry;

namespace Workspace.Managers
{
    public class SelectionManager
    {
        private List<ISelectable> SelectedItems { get; set; } = new List<ISelectable>();

        public SelectionManager()
        {
            // Prenumerera på alla events i SelectionEvents
            SelectionEvents.NodeSelectedEvent += OnNodeSelected;
            SelectionEvents.NodeDeselectedEvent += OnNodeDeselected;
            SelectionEvents.MemberSelectedEvent += OnMemberSelected;
            SelectionEvents.LoadSelectionEvent += OnLoadSelected;
            SelectionEvents.StructureSelectedEvent += OnStructureSelected;
            SelectionEvents.WorkspaceSelectedEvent += OnWorkPointSelected;
            SelectionEvents.WorkPlaneSelectedEvent += OnWorkPlaneSelected;
            SelectionEvents.DeselectAllEvent += ClearSelection;
        }

        public void ClearSelection()
        {
            foreach (var item in SelectedItems)
            {
                item.Selected = false;
            }
            
            SelectedItems.Clear();
        }
        private void OnNodeSelected(TrussNode node)
        {
            if(SelectedItems.Contains(node))
                return;
            
            SelectedItems.Clear();
            SelectedItems.Add(node);
            node.Selected = true;
        }

        private void OnNodeDeselected(TrussNode node)
        {
            if (!SelectedItems.Contains(node))
                return;
            SelectedItems.Remove(node);
            node.Selected = false;
        }

        private void OnMemberSelected(TrussMember member)
        {
        }

        private void OnLoadSelected(PointLoad load)
        {
        }

        private void OnStructureSelected(TrussStructure structure)
        {
        }

        private void OnWorkPointSelected(WorkPoint workPoint)
        {
        }

        private void OnWorkPlaneSelected(WorkPlane workPlane)
        {
        }
    }
}