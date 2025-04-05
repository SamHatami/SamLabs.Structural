using UnityEngine;

public class WorldOrientationView : MonoBehaviour
{
    [SerializeField] private Vector3 screenPosition;

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;

        Vector3 viewportPos = new Vector3(1 - (screenPosition.x / Screen.width),
            1 - (screenPosition.y / Screen.height),
            screenPosition.z);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);


        transform.position = worldPos;
    }
}

