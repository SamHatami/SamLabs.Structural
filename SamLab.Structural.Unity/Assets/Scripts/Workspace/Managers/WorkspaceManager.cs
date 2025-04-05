using System.Collections.Generic;
using Assets.Scripts.Workspace.Factories;
using Assets.Scripts.Workspace.Geometry.ReferenceGeometry;
using Assets.Scripts.Workspace.UI;
using UnityEngine;

namespace Assets.Scripts.Workspace.Managers
{
    public class WorkspaceManager : MonoBehaviour
    {
        [Header("Collections")] [SerializeField]
        public List<WorkPlane> Workplanes;

        [SerializeField] private List<RotateTranslateGizmo> Gizmos;
        [SerializeField] private List<WorkAxis> Workaxes;

        [Header("Base Planes")] 
        [SerializeField] public GameObject XYPlane;
        [SerializeField] public GameObject YZPlane;
        [SerializeField] public GameObject XZPlane;

        [Header("Factories and handlers")] 
        [SerializeField] private WorkSpaceFactory _workSpaceFactory;

        [SerializeField] private WorkspaceSnapHandler _snapHandler;

        private WorkspaceSettings _settings;

        void Awake()
        {
            if (XYPlane != null) XYPlane.GetComponent<BasePlane>().Initialize();
            if (YZPlane != null) YZPlane.GetComponent<BasePlane>().Initialize();
            if (XZPlane != null) XZPlane.GetComponent<BasePlane>().Initialize();
        }

        private void Start()
        {
            _settings = new WorkspaceSettings();
     

        }

        public void CreateNewWorkPlane() //Pass enum to choose which creationform
        {
            var sourcePLane = XYPlane.GetComponent<BasePlane>();
            var wp = _workSpaceFactory.CreateWorkPlaneFromOffset(sourcePLane, 2, true);
            Workplanes.Add(wp);
            _snapHandler.ActiveWorkPlane = wp;

            UnityEngine.Camera.main.transform.LookAt(_snapHandler.ActiveWorkPlane.Origo);
        }

        public WorkspaceSettings GetSnapSettings()
        {
            return _settings;
        }

        public void AddWorkPlane()
        {
        }
    }
}