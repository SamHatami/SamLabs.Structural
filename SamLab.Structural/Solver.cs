using System.Numerics;
using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Core;

public static class Solver
{
    //TODO: Perhaps in the future there will be some options, but they will be read during the operations.

    //TODO: A PreSolver that will check if structure is solvable using current methods. (statically determined)
    public static IStructure Solve(IStructure structure)
    {
        switch (structure)
        {
            case TrussStructure trussStructure:
                SolveTrussStructure(trussStructure);
                break;
        }

        return structure;
    }

    private static TrussStructure SolveTrussStructure(TrussStructure trussStructure)
    {
        //Calculate reaction forces


        Equilibrium(trussStructure);
        //walk through each joint and calculation joint forces by method of joints

        foreach (var node in trussStructure.Nodes)
        {
        }

        //Collect

        return trussStructure;
    }

    private static void Equilibrium(TrussStructure structure)
    {
        //split into x and y components
        float sumXForces = structure.Forces.Sum(force => force.X);
        float sumYForces = structure.Forces.Sum(force => force.Y);

        //Select a boundary node to sum up moments, a node that has maximum of two members connected to it

        //Calculate moments

        Node? momentEquilibriumNode = null;

        foreach (var boundaryCondition in structure.BoundaryConditions)
        {
            if (boundaryCondition.Node.Members.Count == 2)
                momentEquilibriumNode = boundaryCondition.Node; //select this node as moment equilibrium node

        }
        //Settings up an equation for forces and moments, to solve the unknowns

        //from each force, get the distance from the node and multiply by the perpendicular force component
        //sum them up

    }
}
