using UnityEngine;

public class CreateMaterialInstance : MonoBehaviour
{
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Color _color;

    void Start()
    {
        Material instanceMaterial = new Material(baseMaterial);

        instanceMaterial.color = _color;

        GetComponent<Renderer>().material = instanceMaterial;
    }
}
