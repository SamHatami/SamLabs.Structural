using UnityEditor.Toolbars;
using UnityEngine;

public class ScaleToCamera : MonoBehaviour
{
    [SerializeField] private float _worldScale = 1;
    [SerializeField] private bool LockXScale= false;
    [SerializeField] private bool LockYScale= false;
    [SerializeField] private bool LockZScale= false;

    private float oldCameraSize = 0;

    void Awake()
    {
        SetScale();
    }

    void OnEnable()
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

    void Update()
    {
        if(oldCameraSize == Camera.main.orthographicSize)
        {
            return;
        }

        oldCameraSize = UnityEngine.Camera.main.orthographicSize;

        SetScale();

    }


    

}
