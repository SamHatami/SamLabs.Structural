using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    public Vector3 Position;
    public Vector3 Forces;
}
