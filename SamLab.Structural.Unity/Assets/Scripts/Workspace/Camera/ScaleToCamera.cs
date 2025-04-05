using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    [SerializeField] private float _worldScale = 1;
    void Start()
    {
        transform.localScale = new Vector3(_worldScale, _worldScale, _worldScale);
    }

    void Update()
    {
        var screenSpaceScale = (_worldScale / 10) * UnityEngine.Camera.main.orthographicSize;
        transform.localScale = new Vector3(screenSpaceScale, screenSpaceScale, screenSpaceScale);

    }
}
