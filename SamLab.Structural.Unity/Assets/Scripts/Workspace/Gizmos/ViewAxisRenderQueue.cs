using UnityEngine;

public class ViewAxisRenderQueue : MonoBehaviour
{
    [SerializeField] private int renderQueue = 4000;

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Material materialInstance = new Material(renderer.material);

            materialInstance.renderQueue = renderQueue;

            renderer.material = materialInstance;
        }
    }
}
