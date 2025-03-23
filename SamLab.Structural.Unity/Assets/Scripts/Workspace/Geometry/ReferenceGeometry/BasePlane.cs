using System;
using Assets.Scripts.Workspace.Geometry.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Workspace.Geometry.ReferenceGeometry
{
    public class BasePlane : MonoBehaviour, IReferenceGeometry, IPlane
    {
        [SerializeField] private BasePlaneEnum plane;

        public BasePlaneEnum SelectedBasePlane
        {
            get => plane;
            set => plane = value;
        }

        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }

        public Vector3 Direction1 { get; set; }
        public Vector3 Direction2 { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Origo { get; set; } = Vector3.zero;
        public Quaternion Rotation { get; set; }


        public void Initialize()
        {
            throw new NotImplementedException();
        }


        private void Start()
        {
            switch (SelectedBasePlane)
            {
                case BasePlaneEnum.XY:
                    Direction1 = Vector3.right;
                    Direction2 = Vector3.up;
                    Normal = Vector3.forward;
                    name = "XY Plane";
                    Rotation = Quaternion.Euler(0,0,0);
                    break;
                case BasePlaneEnum.YZ:
                    Direction1 = Vector3.up;
                    Direction2 = Vector3.forward;
                    Normal = Vector3.right;
                    name = "YZ Plane";
                    Rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case BasePlaneEnum.XZ:
                    name = "XZ Plane";
                    Direction1 = Vector3.right;
                    Direction2 = Vector3.forward;
                    Normal = Vector3.up;
                    Rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }

        }
    }
}