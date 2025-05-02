using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class InputCoordinates : VisualElement
{
    [SerializeField, DontCreateProperty]
    private float _Coord;
    [UxmlAttribute, CreateProperty]
    public float Coordinate { 
        get => _Coord;
        set => _Coord = value;
    }
}
