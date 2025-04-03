using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Structure.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Structure.Base.Loads
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
