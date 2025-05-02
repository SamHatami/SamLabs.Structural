using UnityEngine;

namespace Workspace.Gizmos
{
    public class TranslateGizmo : MonoBehaviour
    {
        [SerializeField] private Transform _attachedTransform;


        private void Awake()
        {
        }

        private void Update()
        {
        }


        private void OnMouseDrag()
        {
            if (_attachedTransform == null)
                return;
            var mousePos = Input.mousePosition;
            mousePos.z = 10f; // Distance from the camera
            var worldPos = UnityEngine.Camera.main.ScreenToWorldPoint(mousePos);
            _attachedTransform.position = new Vector3(worldPos.x, worldPos.y, _attachedTransform.position.z);
        }
    }
}