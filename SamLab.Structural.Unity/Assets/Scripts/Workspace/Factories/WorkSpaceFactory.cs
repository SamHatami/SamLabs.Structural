using Assets.Scripts.Workspace.Geometry.Interfaces;
using Assets.Scripts.Workspace.Geometry.ReferenceGeometry;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

namespace Assets.Scripts.Workspace.Factories
{

    public  class WorkSpaceFactory:MonoBehaviour
    {
        [SerializeField] private GameObject _workPlanePrefab;
        [SerializeField] private GameObject _workAxisPrefab;
        [SerializeField] private GameObject _workPointPrefab;

        [SerializeField] private GameObject _workPlaneCollection;
        [SerializeField] private GameObject _workAxisCollection;
        [SerializeField] private GameObject _workPointCollection;
        public WorkPlane CreateWorkPlaneFromOffset(IPlane sourcePlane, float offset, bool activate)
        {
            Vector3 finalPosition = sourcePlane.Origo + sourcePlane.Normal*offset;


            var workPlaneObj = Instantiate(_workPlanePrefab, finalPosition, sourcePlane.Rotation);

            workPlaneObj.transform.SetParent(_workPlaneCollection.transform);

            return workPlaneObj.GetComponent<WorkPlane>();

        }
    }
}
