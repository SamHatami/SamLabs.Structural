using UnityEngine;

namespace Workspace.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        private Vector3 _oldCameraPosition;

        private void Awake()
        {
            transform.LookAt(UnityEngine.Camera.main.transform);
        }

        private void Update()
        {
            transform.LookAt(UnityEngine.Camera.main.transform);
            transform.rotation = UnityEngine.Camera.main.transform.rotation;
            //transform.Rotate(Vector3.up,angle);
        }
    }
}