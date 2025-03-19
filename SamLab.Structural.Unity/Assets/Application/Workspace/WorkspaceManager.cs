using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Application.Workspace
{
    public class WorkspaceManager : MonoBehaviour
    {
        [SerializeField] public List<WorkPlane> Workplanes;
        [SerializeField] List<RotateTranslateGizmo> Gizmos;
        [SerializeField] List<WorkAxis> Workaxes;

        private WorkspaceSnapSettings _snapSettings;

        private void Start()
        {
            _snapSettings = new WorkspaceSnapSettings();
            //alternativliy this should be loaded from a file
        }

        public WorkspaceSnapSettings GetSnapSettings()
        {
            return _snapSettings;
        }
    }
}


