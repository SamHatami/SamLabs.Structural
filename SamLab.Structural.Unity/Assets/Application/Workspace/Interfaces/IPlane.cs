using UnityEngine;

namespace Assets.Application.Workspace.Interfaces
{
    public interface IPlane
    {
        public Vector3 Direction1 { get; set; }
        public Vector3 Direction2 { get; set; }

        public Vector3 Origo { get; set; }
    }
}