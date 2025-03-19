using Assets.Application.Workspace.Interfaces;
using System;
using UnityEngine;

namespace Assets.Application.Workspace.ReferenceGeometry
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
        public Vector3 Origo { get; set; } = Vector3.zero;


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
                    name = "XY Plane";
                    transform.SetPositionAndRotation(Origo,Quaternion.Euler(0,0,90));
                    break;
                case BasePlaneEnum.YZ:
                    Direction1 = Vector3.up;
                    Direction2 = Vector3.forward;
                    name = "YZ Plane";
                    transform.SetPositionAndRotation(Origo, Quaternion.Euler(90, 0, 0));
                    break;
                case BasePlaneEnum.XZ:
                    name = "XZ Plane";
                    Direction1 = Vector3.right;
                    Direction2 = Vector3.forward;
                    transform.SetPositionAndRotation(Origo, Quaternion.Euler(0, 0, 0));
                    break;
            }

        }
    }
}