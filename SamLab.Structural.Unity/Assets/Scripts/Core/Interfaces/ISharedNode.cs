using Assets.Scripts.Structure.Managers;

namespace Assets.Scripts.Core.Interfaces
{
    public interface ISharedNode
    {
        /*
         * When connecting two structure shared nodes will be created, the referenced nodes are removed
         * If the shared node is disconnected, either by when a structure is moved or deleted this node is transformed into two normal nodes.
         * The user should be prompted to disconnect the structures if deletion or move is beeing made.
         * Some utils should be created to aid the user to re-connect structures, by moving nodes (elongating or shortening members) or moving one of the structures entirely.
         */
        TrussStructure TrussStructure1 { get; set; }
        TrussStructure TrussStructure2 { get; set; }
        bool IsConnected { get; set; }

    }
}