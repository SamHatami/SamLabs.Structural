using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Vector3 _oldCameraPosition;
    void Awake()
    {
        transform.LookAt(Camera.main.transform);
    }
    void Update()
    {

        transform.LookAt(Camera.main.transform);
        transform.rotation = Camera.main.transform.rotation;
        //transform.Rotate(Vector3.up,angle);
    }
}
