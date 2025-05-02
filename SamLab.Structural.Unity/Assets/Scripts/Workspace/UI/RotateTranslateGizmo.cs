using System.Collections.Generic;
using UnityEngine;

namespace Workspace.UI
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