using System;
using Structure.Interfaces;
using UnityEngine;

namespace Structure.Base.Loads
{
    internal class LineLoad : MonoBehaviour, ILineLoad
    {
        public float LoadPerUnitLength { get; set; }

        public Vector3 Direction { get; set; }
        public Vector3 StartNode { get; set; }
        public Vector3 EndNode { get; set; }

        public float GetStartNodeLoad()
        {
            throw new NotImplementedException();
        }

        public float GetEndNodeLoad()
        {
            throw new NotImplementedException();
        }
    }
}