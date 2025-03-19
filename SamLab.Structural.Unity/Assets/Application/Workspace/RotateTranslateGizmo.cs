using System.Collections.Generic;
using UnityEngine;

namespace Assets.Application.Workspace
{
    public class RotateTranslateGizmo : MonoBehaviour
    {
        [SerializeField] public List<GameObject> attachedObjects;

        private void Start()
        {
            attachedObjects = new List<GameObject>();
        }
    }
}