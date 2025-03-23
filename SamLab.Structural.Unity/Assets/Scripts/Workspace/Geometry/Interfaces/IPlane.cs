using UnityEngine;

namespace Assets.Scripts.Workspace.Geometry.Interfaces
{
    public interface IPlane
    {
        public Vector3 Direction1 { get; set; }
        public Vector3 Direction2 { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Origo { get; set; }
        public Quaternion Rotation { get; set; }
    }
}