using UnityEngine;

namespace Ogxd
{
    public class ZoomOverTime : MonoBehaviour
    {
        public float speedFactor = 1f;
        public Material material;

        private void Update()
        {
            material.SetFloat("_Scale", Mathf.Exp(speedFactor * Time.timeSinceLevelLoad % 10));
        }
    }
}