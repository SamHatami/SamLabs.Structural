using UnityEngine;

namespace Workspace.Camera
{
    public class ScaleToCamera : MonoBehaviour
    {
        [SerializeField] private float _worldScale = 1;
        [SerializeField] private bool LockXScale = false;
        [SerializeField] private bool LockYScale = false;
        [SerializeField] private bool LockZScale = false;

        private float oldCameraSize = 0;

        private void Awake()
        {
            SetScale();
        }

        private void OnEnable()
        {
            SetScale();
        }

        private void SetScale()
        {
            var screenSpaceScale = _worldScale * UnityEngine.Camera.main.orthographicSize;

            transform.localScale = new Vector3(
                LockXScale ? transform.localScale.x : screenSpaceScale,
                LockYScale ? transform.localScale.y : screenSpaceScale,
                LockZScale ? transform.localScale.z : screenSpaceScale);
        }

        private void Update()
        {
            if (oldCameraSize == UnityEngine.Camera.main.orthographicSize) return;

            oldCameraSize = UnityEngine.Camera.main.orthographicSize;

            SetScale();
        }
    }
}