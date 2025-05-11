using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Workspace.Interaction
{
    public class HoverColorChange : MonoBehaviour
    {
        public Color BaseColor = Color.white;

        [Header("Hover Color Settings")] public Color HoverColor = Color.white;
        [Header("Selected Color Settings")] public Color SelectedColor = Color.blue;
        private Collider _collider;
        private Material material;
        public bool setHovered { get; set; }
        
        private void Awake()
        {
            //TODO: In the future read colors from workspace settings
        }

        private void Start()
        {
            material = GetComponent<Renderer>().material;
            _collider = GetComponent<Collider>();
            material.color = BaseColor;
            material.SetColor("_EmissionColor", HoverColor);
        }

        private void Update()
        {
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (setHovered)
                HoverColorOn();
            else
                HoverColorOff();
            
        }
        private void OnMouseEnter()
        {
            setHovered = true;
        }

        private void OnMouseExit()
        {
            setHovered = false;
        }

        private void HoverColorOff()
        {
            if(material == null) return;
            
            material.DisableKeyword("_EMISSION");
        }

        private void HoverColorOn()
        {
            if (material == null) return; //Might add alternative to highlight all childern
            
            material.EnableKeyword("_EMISSION");
        }
    }
}