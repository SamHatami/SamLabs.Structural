﻿using Core.Interfaces;
using Structure.Base.Constraints;

namespace Workspace.Geometry.Interfaces
{
    public interface IConstructionGeometry
    {
        bool IsVisible { get; set; }
        bool IsActive { get; set; }

        IConstraint[] Constraints { get; set; }
    }
}