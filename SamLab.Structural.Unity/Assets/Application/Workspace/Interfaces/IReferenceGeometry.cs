using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Application.Workspace.Interfaces
{
    public interface IReferenceGeometry
    {
        void Initialize();
        string Name { get; set; }
        bool IsVisible { get; set; }
        bool IsActive { get; set; }
    }
}
