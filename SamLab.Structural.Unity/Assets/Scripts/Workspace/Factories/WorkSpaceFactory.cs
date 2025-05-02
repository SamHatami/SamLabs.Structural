using UnityEngine;
using Workspace.Geometry.Interfaces;
using Workspace.Geometry.ReferenceGeometry;

namespace Workspace.Factories
{
    public class WorkSpaceFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _workPlanePrefab;
        [SerializeField] private GameObject _workAxisPrefab;
        [SerializeField] private GameObject _workPointPrefab;

        [SerializeField] private GameObject _workPlaneCollection;
        [SerializeField] private GameObject _workAxisCollection;
        [SerializeField] private GameObject _workPointCollection;

        public WorkPlane CreateWorkPlaneFromOffset(IPlane sourcePlane, float offset, bool activate)
        {
            var finalPosition = sourcePlane.Origo + sourcePlane.Normal * offset;


            var workPlaneObj = Instantiate(_workPlanePrefab, finalPosition, sourcePlane.Rotation);

            workPlaneObj.transform.SetParent(_workPlaneCollection.transform);

            return workPlaneObj.GetComponent<WorkPlane>();
        }
    }
}