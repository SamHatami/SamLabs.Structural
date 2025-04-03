using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Structure.Interfaces
{
    internal interface ILineLoad : IDistributedLoad
    {
        float LoadPerUnitLength { get; set; }

        //TBD: Add more properties and methods
    }
}
