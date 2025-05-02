using UnityEngine;

namespace Workspace.Gizmos
{
    public class ViewAxisRenderQueue : MonoBehaviour
    {
        [SerializeField] private int renderQueue = 4000;

        private void Awake()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                var materialInstance = new Material(renderer.material);

                materialInstance.renderQueue = renderQueue;

                renderer.material = materialInstance;
            }
        }
    }
}