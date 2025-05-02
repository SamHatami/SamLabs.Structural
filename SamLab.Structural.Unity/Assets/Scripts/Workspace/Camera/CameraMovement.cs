using UnityEngine;

namespace Workspace.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("REFERENCES")] [Header("Camera")]
        public UnityEngine.Camera MainCamera;

        [Range(0f, 50f)] public float CameraFocusPointDistance = 25f;

        [Header("SETTINGS")] [Space(2)] [Header("Speed")] [Range(0f, 1f)] [Tooltip("The Movement Speed of Camera")]
        public float MovementSpeed;

        [Range(0f, 1f)] [Tooltip("The On-Axis Rotation Speed of Camera")]
        public float RotationSpeed;

        [Range(0f, 15f)] [Tooltip("The Zoom speed of Camera")]
        public float ZoomSpeed;

        [Range(45f, 450f)] [Tooltip("The Revolution speed of Camera")]
        public float RevolutionSpeed;

        [Space(2)]
        [Header("Adjusting Values")]
        [Range(0f, 1f)]
        [Tooltip("The amount the Movement speed will be affected by Zoom Input")]
        public float ZoomInputAdjust = 1f;


        //Internal Private Variables
        private float ZoomInputIndex = 1f;
        private RaycastHit hit; //For Storing Focus Point

        private void Update()
        {
            //Translation
            var translateX = 0f;
            var translateY = 0f;

            //Rotation
            var rotationX = 0f;
            var rotationY = 0f;

            var ZoomInput = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            MainCamera.orthographicSize -= ZoomInput;

            ZoomInputIndex += ZoomInput * ZoomInputAdjust;

            if (Input.GetMouseButton(2))
            {
                translateY = Input.GetAxis("Mouse Y") * (MovementSpeed / ZoomInputIndex);
                translateX = Input.GetAxis("Mouse X") * (MovementSpeed / ZoomInputIndex);
            }
            else if (Input.GetMouseButton(1))
            {
                rotationY = Input.GetAxis("Mouse Y") * RotationSpeed;
                rotationX = Input.GetAxis("Mouse X") * RotationSpeed;
            }

            else if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftShift))
            {
                var revY = Input.GetAxis("Mouse Y");
                var revX = Input.GetAxis("Mouse X");


                transform.RotateAround(GetFocusPoint(), Vector3.up, revX * RevolutionSpeed * Time.deltaTime);
                transform.RotateAround(GetFocusPoint(), transform.right, -revY * RevolutionSpeed * Time.deltaTime);
            }

            transform.Translate(translateX, translateY, ZoomInput);
            transform.Rotate(0, rotationX, 0, Space.World);
            transform.Rotate(-rotationY, 0, 0);
        }

        private Vector3 GetFocusPoint()
        {
            if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit,
                    CameraFocusPointDistance))
                return hit.point;
            else
                return MainCamera.transform.position + MainCamera.transform.forward * CameraFocusPointDistance;
        }

        [ContextMenu("Auto Assign Variables")]
        public void AutoSetup()
        {
            MainCamera = UnityEngine.Camera.main;
        }
    }
}