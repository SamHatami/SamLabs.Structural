using UnityEngine;

namespace Workspace.Gizmos
{
    public class CreateMaterialInstance : MonoBehaviour
    {
        [SerializeField] private Material baseMaterial;
        [SerializeField] private Color _color;

        private void Start()
        {
            var instanceMaterial = new Material(baseMaterial);

            instanceMaterial.color = _color;

            GetComponent<Renderer>().material = instanceMaterial;
        }
    }
}