using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Core;

public static class Solver
{
    //TODO: Perhaps in the future there will be some options, but they will be read during the operations.

    //TODO: A PreSolver that will check if structure is solvable using current methods. (statically determinant)
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

        //walk through each joint and calculation joint forces by method of joints

        foreach (var node in trussStructure.Nodes)
        {
        }

        //Collect

        return trussStructure;
    }
}