using System;
using System.Collections.Generic;
using Core.Interfaces;
using Structure.Base;
using Structure.Base.Loads;
using Structure.Managers;
using Workspace.Geometry.ReferenceGeometry;

namespace Workspace.Interaction
{
    public static class SelectionEvents
    {
        public static event Action<TrussNode> NodeSelectedEvent;
        public static event Action<TrussNode> NodeDeselectedEvent;
        public static event Action<TrussMember> MemberSelectedEvent;
        public static event Action<PointLoad> LoadSelectionEvent;
        public static event Action<TrussStructure> StructureSelectedEvent;
        public static event Action<WorkPoint> WorkspaceSelectedEvent;
        public static event Action<WorkPlane> WorkPlaneSelectedEvent;
        public static event Action DeselectAllEvent;

        
        public static void PublishIStructuralElementSelected(IStructuralElement element)
        {

            switch (element)
            {
                case TrussNode trussNode:
                    PublishNodeSelected(trussNode);
                    break;
            }
        }

        public static void DeselectAll()
        {
            DeselectAllEvent?.Invoke();
        }
        
        public static void PublishNodeSelected(TrussNode node)
        {

            NodeSelectedEvent?.Invoke(node);
        }

        public static void PublishNodeDeselected(TrussNode node)
        {
            NodeDeselectedEvent?.Invoke(node);
        }

        public static void PublishMemberSelected(TrussMember member)
        {
            MemberSelectedEvent?.Invoke(member);
        }

        public static void PublishLoadSelected(PointLoad load)
        {
            LoadSelectionEvent?.Invoke(load);
        }

        public static void PublishStructureSelected(TrussStructure structure)
        {
            StructureSelectedEvent?.Invoke(structure);
        }

        public static void PublishWorkPointSelected(WorkPoint workspace)
        {
            WorkspaceSelectedEvent?.Invoke(workspace);
        }

        public static void PublishWorkPlaneSelectedEvent(WorkPlane workplane)
        {
            WorkPlaneSelectedEvent?.Invoke(workplane);
        }
        
        
    }
}