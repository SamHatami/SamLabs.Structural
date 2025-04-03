using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Workspace.Gizmos
{
    public class TranslateGizmo : MonoBehaviour
    {
        [SerializeField] private Transform _attachedTransform;

        [SerializeField] private float _worldScale = 20;

        void Awake()
        {
            transform.localScale = new Vector3(_worldScale, _worldScale, _worldScale);
        }

        void Update()
        {
            var screenSpaceScale = (_worldScale/100) * UnityEngine.Camera.main.orthographicSize;
            transform.localScale = new Vector3(screenSpaceScale, screenSpaceScale, screenSpaceScale);
        }
    }
}
