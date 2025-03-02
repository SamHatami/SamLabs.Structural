using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Core;

public static class Solver
{
    //TODO: Perhaps in the future there will be some options, but they will be read during the operations.

    //TODO: A PreSolver that will check if structure is solvable using current methods. (statically determined). Read Below:
    //TODO: External/Internal redundancy & instability. If external redundancy,give warning: if internal redundancy, give error . If External instability, give error: if internal instability, give error.
    //TODO: 
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

        //Sort the nodes by id 
        trussStructure.Members.Sort((a, b) => a.Id.CompareTo(b.Id));

        trussStructure.Solve();

        return trussStructure;
    }

    private static void Equilibrium(TrussStructure structure) 
    {
        //This was me not understanding that hand calculations are not very useful in this case
        //Select a boundary node to check equilibirum of external forces -> free body diagram
        //Calculate moments

        Node? momentEquilibriumNode = null;

        foreach (var support in structure.Supports)
        {
            if (support.Node.Members.Count != 2) continue;

            momentEquilibriumNode = support.Node;
            break;
        }

        if (momentEquilibriumNode == null)
            //Log error
            //Send log event 
            return;

        float sumFx = structure.ExternalForces.Sum(force => force.X); 
        float sumFy = structure.ExternalForces.Sum(force => force.Y);
        float sumM = structure.ExternalForces.Sum(force => force.CalculateMoment(momentEquilibriumNode.Value)); //2d cross product of r and f is a scalar, in 3d it is a vector -> get the magnitude of the vector to get the scalar

        var A = Matrix<double>.Build.DenseOfArray(new double[,] {
            { 3, 2, -1 },
            { 2, -2, 4 },
            { -1, 0.5, -1 }
        });
        var b = Vector<double>.Build.Dense(new double[] { sumFx, sumFy, sumM });
        var x = A.Solve(b);
        //Settings up an equation for forces and moments, to solve the unknowns

        //TODO: Solve the system of equations -> Ax = b -> Gaussian or LU ?

    }


}
