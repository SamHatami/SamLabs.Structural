
using UnityEngine;

namespace Assets.Scripts.Structure.Interfaces
{
    public interface IDistributedLoad : ILoad
    {
        Vector3 Direction { get; set; }
        Vector3 StartNode { get; set; }
        Vector3 EndNode { get; set; }

        float GetStartNodeLoad();
        float GetEndNodeLoad();
    }
}