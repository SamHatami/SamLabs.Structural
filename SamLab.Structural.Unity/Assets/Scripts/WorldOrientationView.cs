using UnityEngine;

public class WorldOrientationView : MonoBehaviour
{
    [SerializeField] private Vector3 screenPosition;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;

        var viewportPos = new Vector3(1 - screenPosition.x / Screen.width,
            1 - screenPosition.y / Screen.height,
            screenPosition.z);

        var worldPos = Camera.main.ViewportToWorldPoint(viewportPos);


        transform.position = worldPos;
    }
}