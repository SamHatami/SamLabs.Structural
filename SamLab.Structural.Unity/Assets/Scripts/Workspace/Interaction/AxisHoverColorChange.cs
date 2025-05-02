using UnityEngine;

namespace Workspace.Interaction
{
    public class AxisHoverColorChange : MonoBehaviour
    {
        [Header("Hover Color Settings")] public Color HoverColor = Color.white;
        public Color BaseColor = Color.white;

        private Collider axisCollider;
        private Material material;

        private void Awake()
        {
            //TODO: In the future read colors from workspace settings
        }

        private void Start()
        {
            material = GetComponent<Renderer>().material;
            axisCollider = GetComponent<Collider>();
            material.color = BaseColor;
            material.SetColor("_EmissionColor", HoverColor);
        }

        private void Update()
        {
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit) && hit.collider == axisCollider)
                HoverColorOn();
            else
                HoverColorOff();
        }

        private void HoverColorOff()
        {
            material.DisableKeyword("_EMISSION");
        }

        private void HoverColorOn()
        {
            material.EnableKeyword("_EMISSION");
        }
    }
}