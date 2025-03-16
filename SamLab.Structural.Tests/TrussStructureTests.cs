using System.Numerics;
using SamLab.Structural.Core;
using SamLab.Structural.Core.Analysis;
using SamLab.Structural.Core.Analysis.Constraints;
using SamLab.Structural.Core.Elements;
using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Tests
{
    public class TrussStructureTests
    {
        [Fact]
        public void SimpleBeam_IsStaticallyDeterminate()
        {
            // Simple beam with two nodes and one memeberData
            var node1 = new NodeData(new Vector2(0, 0));
            var node2 = new NodeData(new Vector2(3, 0));

            var member = new TrussMemeberData(node1, node2);

            // Pin at one end, roller at the other
            var pinSupport = new Support(
                node1,
                new BoundaryCondition(node1, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var rollerSupport = new Support(
                node2,
                new BoundaryCondition(node2, BoundaryConditionType.Roller),
                Vector2.UnitY);
            rollerSupport.UpdateDegreeOfFreedom(new DoF(false, true, false, false, false, false));

            var trussStructure = new TrussStructure
            {
                Nodes = [node1, node2],
                Members = [member],
                Supports = [pinSupport, rollerSupport],
                ExternalForces = []
            };

            Assert.True(trussStructure.IsStaticallyDeterminate());
        }

        [Fact]
        public void Problem_3_1()
        {
            // Ref: civilengineeronline.com/mech/prob31.htm
            var nodeA = new NodeData(new Vector2(0, 0));
            var nodeB = new NodeData(new Vector2(2, 0));
            var nodeC = new NodeData(new Vector2(4, 0));
            var nodeD = new NodeData(new Vector2(4, 2));
            var nodeE = new NodeData(new Vector2(2, 2));
            var nodeF = new NodeData(new Vector2(0, 2));

            var memberAB = new TrussMemeberData(nodeA, nodeB);
            var memberBC = new TrussMemeberData(nodeB, nodeC);
            var memberCD = new TrussMemeberData(nodeC, nodeD);
            var memberDE = new TrussMemeberData(nodeD, nodeE);
            var memberEF = new TrussMemeberData(nodeE, nodeF);
            var memberFA = new TrussMemeberData(nodeF, nodeA);
            var memberAE = new TrussMemeberData(nodeA, nodeE);
            var memberBE = new TrussMemeberData(nodeB, nodeE);
            var memberCE = new TrussMemeberData(nodeC, nodeE);

            var pinSupportC = new Support(nodeC, new BoundaryCondition(nodeC, BoundaryConditionType.Pinned), Vector2.UnitY);
            var rollerSupportA =
                new Support(nodeA, new BoundaryCondition(nodeA, BoundaryConditionType.Roller), Vector2.UnitY);

            var forceB = new Force(0, -20, nodeB);
            var forceD = new Force(-15, 0, nodeD);
            var forceE = new Force(0, -10, nodeE);
            var forceF = new Force(0, -25, nodeF);

            var trussStructure = new TrussStructure
            {
                Nodes = [nodeA, nodeB, nodeC, nodeD, nodeE, nodeF],
                Members = [memberAB, memberFA, memberAE, memberBC, memberBE, memberCD, memberCE, memberDE, memberEF],
                Supports = [pinSupportC, rollerSupportA],
                ExternalForces = [forceD, forceE, forceF, forceB]
            };

            Assert.True(trussStructure.IsStaticallyDeterminate());

            var results = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < results.Length; i++)
                results[i] = -results[i];

            // Check memeberData forces
            Assert.Equal(-22.5, results[0], 0.01);
            Assert.Equal(25, results[1], 0.01);
            Assert.Equal(31.82, results[2], 0.01);
            Assert.Equal(-22.5, results[3], 0.01);
            Assert.Equal(-20, results[4], 0.01);
            Assert.Equal(0, results[5], 0.01);
            Assert.Equal(10.61, results[6], 0.01);
            Assert.Equal(15, results[7], 0.01);
            Assert.Equal(0, results[8], 0.01);

            // Check reaction forces
            Assert.Equal(15, results[9]);
            Assert.Equal(7.5, results[10]);
            Assert.Equal(47.5, results[11]);
        }

        [Fact]
        //Claude 3.7 Sonnet Generated Truss
        public void SimpleTruss_WithVerticalLoad_CorrectMemberForces()
        {
            // Simple triangular truss
            var nodeA = new NodeData(new Vector2(0, 0));
            var nodeB = new NodeData(new Vector2(4, 0));
            var nodeC = new NodeData(new Vector2(2, 2));

            var memberAB = new TrussMemeberData(nodeA, nodeB);
            var memberAC = new TrussMemeberData(nodeA, nodeC);
            var memberBC = new TrussMemeberData(nodeB, nodeC);

            // Pin at A, roller at B
            var pinSupportA = new Support(
                nodeA,
                new BoundaryCondition(nodeA, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var rollerSupportB = new Support(
                nodeB,
                new BoundaryCondition(nodeB, BoundaryConditionType.Roller),
                Vector2.UnitY);
            rollerSupportB.UpdateDegreeOfFreedom(new DoF(false, true, false, false, false, false));

            // Vertical load at C
            var forceC = new Force(0, -10, nodeC);

            var trussStructure = new TrussStructure
            {
                Nodes = [nodeA, nodeB, nodeC],
                Members = [memberAB, memberAC, memberBC],
                Supports = [pinSupportA, rollerSupportB],
                ExternalForces = [forceC]
            };

            Assert.True(trussStructure.IsStaticallyDeterminate());

            var results = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < results.Length; i++)
                results[i] = -results[i];

            // Check reactions
            Assert.Equal(0, results[reactionStartIndex], 0.01); // Ax = 0
            Assert.Equal(5, results[reactionStartIndex + 1], 0.01); // Ay = 5
            Assert.Equal(5, results[reactionStartIndex + 2], 0.01); // By = 5
        }

        [Fact]
        //Claude 3.7 Sonnet Generated Truss
        public void WarrenTruss_WithDistributedLoad_CorrectReactions()
        {
            // Warren truss with 5 nodes
            var node1 = new NodeData(new Vector2(0, 0));
            var node2 = new NodeData(new Vector2(3, 0));
            var node3 = new NodeData(new Vector2(6, 0));
            var node4 = new NodeData(new Vector2(1.5f, 2));
            var node5 = new NodeData(new Vector2(4.5f, 2));

            var member12 = new TrussMemeberData(node1, node2);
            var member23 = new TrussMemeberData(node2, node3);
            var member14 = new TrussMemeberData(node1, node4);
            var member45 = new TrussMemeberData(node4, node5);
            var member53 = new TrussMemeberData(node5, node3);
            var member24 = new TrussMemeberData(node2, node4);
            var member25 = new TrussMemeberData(node2, node5);

            // Pin at 1, roller at 3
            var pinSupport1 = new Support(
                node1,
                new BoundaryCondition(node1, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var rollerSupport3 = new Support(
                node3,
                new BoundaryCondition(node3, BoundaryConditionType.Roller),
                Vector2.UnitY);
            rollerSupport3.UpdateDegreeOfFreedom(new DoF(false, true, false, false, false, false));

            // Vertical loads simulating distributed load
            var force4 = new Force(0, -5, node4);
            var force5 = new Force(0, -5, node5);

            var trussStructure = new TrussStructure
            {
                Nodes = [node1, node2, node3, node4, node5],
                Members = [member12, member23, member14, member45, member53, member24, member25],
                Supports = [pinSupport1, rollerSupport3],
                ExternalForces = [force4, force5]
            };

            Assert.True(trussStructure.IsStaticallyDeterminate());

            var results = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < results.Length; i++)
                results[i] = -results[i];

            // Check reactions
            Assert.Equal(0, results[reactionStartIndex], 0.01); // R1x = 0
            Assert.Equal(5, results[reactionStartIndex + 1], 0.01); // R1y = 5
            Assert.Equal(5, results[reactionStartIndex + 2], 0.01); // R3y = 5
        }

        [Fact]
        //Claude 3.7 Sonnet Generated Truss
        public void Cantilever_WithPointLoad_CorrectForces()
        {
            // Cantilever truss
            var nodeA = new NodeData(new Vector2(0, 0));
            var nodeB = new NodeData(new Vector2(0, 3));
            var nodeC = new NodeData(new Vector2(3, 0));
            var nodeD = new NodeData(new Vector2(3, 3));
            var nodeE = new NodeData(new Vector2(6, 0));
            var nodeF = new NodeData(new Vector2(6, 3));

            // Members (one removed to make it statically determinate)
            var memberAB = new TrussMemeberData(nodeA, nodeB);
            var memberAC = new TrussMemeberData(nodeA, nodeC);
            var memberBC = new TrussMemeberData(nodeB, nodeC);
            var memberBD = new TrussMemeberData(nodeB, nodeD);
            var memberCD = new TrussMemeberData(nodeC, nodeD);
            var memberCE = new TrussMemeberData(nodeC, nodeE);
            var memberDE = new TrussMemeberData(nodeD, nodeE);
            var memberEF = new TrussMemeberData(nodeE, nodeF);
            // Removed memberDF

            // Pin at A, roller at B
            var supportA = new Support(
                nodeA,
                new BoundaryCondition(nodeA, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var supportB = new Support(
                nodeB,
                new BoundaryCondition(nodeB, BoundaryConditionType.Roller),
                Vector2.UnitY);
            supportB.UpdateDegreeOfFreedom(new DoF(true, false, false, false, false, false));

            // Load at end
            var forceF = new Force(0, -10, nodeF);

            var trussStructure = new TrussStructure
            {
                Nodes = [nodeA, nodeB, nodeC, nodeD, nodeE, nodeF],
                Members = [memberAB, memberAC, memberBC, memberBD, memberCD, memberCE, memberDE, memberEF],
                Supports = [supportA, supportB],
                ExternalForces = [forceF]
            };

            Assert.True(!trussStructure.IsStaticallyDeterminate());

            // Add dummy memeberData to make it statically determinate
            var dummyMember = new TrussMemeberData(nodeD, nodeF);
            trussStructure.Members.Add(dummyMember);

            Assert.True(trussStructure.IsStaticallyDeterminate());

            var results = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < results.Length; i++)
                results[i] = -results[i];

            // Check total vertical reaction equals applied load
            Assert.Equal(10.0, results[reactionStartIndex + 1], 0.1);

            // Check horizontal reactions sum to zero
            Assert.True(results[reactionStartIndex] + results[reactionStartIndex + 2] == 0);
        }

        [Fact]
        //Claude 3.7 Sonnet Generated Truss
        public void BridgeTruss_WithMovingLoad_CorrectMemberForces()
        {
            // Bridge truss with 5 nodes
            var nodeA = new NodeData(new Vector2(0, 0));
            var nodeB = new NodeData(new Vector2(4, 0));
            var nodeC = new NodeData(new Vector2(8, 0));
            var nodeD = new NodeData(new Vector2(2, 2));
            var nodeE = new NodeData(new Vector2(6, 2));

            var memberAB = new TrussMemeberData(nodeA, nodeB);
            var memberBC = new TrussMemeberData(nodeB, nodeC);
            var memberAD = new TrussMemeberData(nodeA, nodeD);
            var memberBD = new TrussMemeberData(nodeB, nodeD);
            var memberBE = new TrussMemeberData(nodeB, nodeE);
            var memberCE = new TrussMemeberData(nodeC, nodeE);
            var memberDE = new TrussMemeberData(nodeD, nodeE);

            // Pin at A, roller at C
            var pinSupportA = new Support(
                nodeA,
                new BoundaryCondition(nodeA, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var rollerSupportC = new Support(
                nodeC,
                new BoundaryCondition(nodeC, BoundaryConditionType.Roller),
                Vector2.UnitY);
            rollerSupportC.UpdateDegreeOfFreedom(new DoF(false, true, false, false, false, false));

            // Load at position B
            var forceB = new Force(0, -10, nodeB);

            var trussStructure = new TrussStructure
            {
                Nodes = [nodeA, nodeB, nodeC, nodeD, nodeE],
                Members = [memberAB, memberBC, memberAD, memberBD, memberBE, memberCE, memberDE],
                Supports = [pinSupportA, rollerSupportC],
                ExternalForces = [forceB]
            };

            Assert.True(trussStructure.IsStaticallyDeterminate());

            var resultsWithLoadAtB = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < resultsWithLoadAtB.Length; i++)
                resultsWithLoadAtB[i] = -resultsWithLoadAtB[i];

            // Check reactions with load at B
            Assert.Equal(0, resultsWithLoadAtB[reactionStartIndex], 0.01); // RAx = 0
            Assert.Equal(5, resultsWithLoadAtB[reactionStartIndex + 1], 0.01); // RAy = 5
            Assert.Equal(5, resultsWithLoadAtB[reactionStartIndex + 2], 0.01); // RCy = 5

            // Test with load at position E
            trussStructure.ExternalForces.Clear();
            var forceE = new Force(0, -10, nodeE);
            trussStructure.ExternalForces.Add(forceE);

            var resultsWithLoadAtE = trussStructure.Solve();

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < resultsWithLoadAtB.Length; i++)
                resultsWithLoadAtE[i] = -resultsWithLoadAtE[i];

            // Check reactions with load at E
            Assert.Equal(0, resultsWithLoadAtE[reactionStartIndex], 0.01); // RAx = 0
            Assert.Equal(2.5, resultsWithLoadAtE[reactionStartIndex + 1], 0.01); // RAy = 2.5
            Assert.Equal(7.5, resultsWithLoadAtE[reactionStartIndex + 2], 0.01); // RCy = 7.5
        }

        [Fact]
        //Claude 3.7 Sonnet Generated Truss
        public void LargeRoofTruss_WithMultipleLoads_CorrectMemberForces()
        {
            // Large roof truss with 9 nodes (modified Fink/Howe truss configuration)
            var nodeA = new NodeData(new Vector2(0, 0));           // Left support
            var nodeB = new NodeData(new Vector2(12, 0));          // Right support
            var nodeC = new NodeData(new Vector2(3, 0));           // Bottom chord
            var nodeD = new NodeData(new Vector2(6, 0));           // Bottom center
            var nodeE = new NodeData(new Vector2(9, 0));           // Bottom chord
            var nodeF = new NodeData(new Vector2(3, 3));           // Left top chord
            var nodeG = new NodeData(new Vector2(6, 4));           // Center top
            var nodeH = new NodeData(new Vector2(9, 3));           // Right top chord

            // Create truss members (13 total - purposely making it statically determinate)
            // Bottom chord
            var memberAC = new TrussMemeberData(nodeA, nodeC);
            var memberCD = new TrussMemeberData(nodeC, nodeD);
            var memberDE = new TrussMemeberData(nodeD, nodeE);
            var memberEB = new TrussMemeberData(nodeE, nodeB);

            // Top chord
            var memberFG = new TrussMemeberData(nodeF, nodeG);
            var memberGH = new TrussMemeberData(nodeG, nodeH);

            // Web members
            var memberAF = new TrussMemeberData(nodeA, nodeF);      // Left diagonal
            var memberCF = new TrussMemeberData(nodeC, nodeF);      // Left vertical
            var memberCG = new TrussMemeberData(nodeC, nodeG);      // Left diagonal
            var memberDG = new TrussMemeberData(nodeD, nodeG);      // Center vertical
            var memberEG = new TrussMemeberData(nodeE, nodeG);      // Right diagonal
            var memberEH = new TrussMemeberData(nodeE, nodeH);      // Right vertical
            var memberBH = new TrussMemeberData(nodeB, nodeH);      // Right diagonal

            // Pin at A, roller at B
            var pinSupportA = new Support(
                nodeA,
                new BoundaryCondition(nodeA, BoundaryConditionType.Pinned),
                Vector2.UnitY);

            var rollerSupportB = new Support(
                nodeB,
                new BoundaryCondition(nodeB, BoundaryConditionType.Roller),
                Vector2.UnitY);
            rollerSupportB.UpdateDegreeOfFreedom(new DoF(false, true, false, false, false, false));

            // Multiple loads to simulate roof loading
            var forceG = new Force(0, -15, nodeG);    // Vertical load at peak
            var forceF = new Force(0, -5, nodeF);     // Load on left top chord
            var forceH = new Force(0, -5, nodeH);     // Load on right top chord
            var forceD = new Force(0, -10, nodeD);    // Load on bottom chord

            var trussStructure = new TrussStructure
            {
                Nodes = [nodeA, nodeB, nodeC, nodeD, nodeE, nodeF, nodeG, nodeH],
                Members = [
                    memberAC, memberCD, memberDE, memberEB,
                    memberFG, memberGH,
                    memberAF, memberCF, memberCG, memberDG, memberEG, memberEH, memberBH
                ],
                Supports = [pinSupportA, rollerSupportB],
                ExternalForces = [forceG, forceF, forceH, forceD]
            };

            // Verify the truss is statically determinate by checking:
            // m + r = 2j for a planar truss
            // where m = number of members, r = number of reactions, j = number of joints
            int members = trussStructure.Members.Count;
            int reactions = 3; // Pin support (2) + roller support (1)
            int joints = trussStructure.Nodes.Count;

            Assert.Equal(2 * joints, members + reactions);
            Assert.True(trussStructure.IsStaticallyDeterminate());

            var results = trussStructure.Solve();
            var reactionStartIndex = trussStructure.Members.Count;

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < results.Length; i++)
                results[i] = -results[i];

            // Total applied load is 35 kN (15+5+5+10)
            // Check if vertical reactions sum to total applied load
            Assert.Equal(35.0, results[reactionStartIndex + 1] + results[reactionStartIndex + 2], 0.1);

            // Check if horizontal reactions sum to zero
            Assert.Equal(0.0, results[reactionStartIndex], 0.1);

            // Check specific expected memeberData forces
            // Bottom chord members should be in tension
            Assert.True(results[0] < 0); // AC in tension
            Assert.True(results[1] < 0); // CD in tension
            Assert.True(results[2] < 0); // DE in tension
            Assert.True(results[3] < 0); // EB in tension

            // Top chord members should be in compression
            Assert.True(results[4] > 0); // FG in compression
            Assert.True(results[5] > 0); // GH in compression

            // Calculate influence of moving the load
            // Move load from G to D (redistributing the 15kN)
            trussStructure.ExternalForces.Clear();
            forceD = new Force(0, -25, nodeD);     // Increased load at center bottom
            forceF = new Force(0, -6, nodeF);      // Same load on left
            forceH = new Force(0, -5, nodeH);      // Same load on right

            trussStructure.ExternalForces.Add(forceD);
            trussStructure.ExternalForces.Add(forceF);
            trussStructure.ExternalForces.Add(forceH);

            var resultsWithCenterLoad = trussStructure.Solve();

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < resultsWithCenterLoad.Length; i++)
                resultsWithCenterLoad[i] = -resultsWithCenterLoad[i];

            // Check if total reaction still equals total applied load (still 35 kN)
            Assert.Equal(36.0, resultsWithCenterLoad[reactionStartIndex + 1] + resultsWithCenterLoad[reactionStartIndex + 2], 0.1);

            // Check that forces in diagonal members change significantly when load position changes
            Assert.NotEqual(results[8], resultsWithCenterLoad[8], 0.1);  // Force in CG changes
            Assert.NotEqual(results[10], resultsWithCenterLoad[10], 0.1); // Force in EG changes

            // Test with asymmetric loading
            trussStructure.ExternalForces.Clear();
            var forceF2 = new Force(0, -20, nodeF);    // Heavier load on left
            var forceH2 = new Force(0, -5, nodeH);     // Lighter load on right
            var forceG2 = new Force(0, -10, nodeG);    // Medium load at center

            trussStructure.ExternalForces.Add(forceF2);
            trussStructure.ExternalForces.Add(forceH2);
            trussStructure.ExternalForces.Add(forceG2);

            var resultsWithAsymmetricLoad = trussStructure.Solve();

            // Reverse reaction forces
            for (var i = reactionStartIndex; i < resultsWithAsymmetricLoad.Length; i++)
                resultsWithAsymmetricLoad[i] = -resultsWithAsymmetricLoad[i];

            // With asymmetric loading, reaction at A should be larger than at B
            Assert.True(resultsWithAsymmetricLoad[reactionStartIndex + 1] >
                        resultsWithAsymmetricLoad[reactionStartIndex + 2]);

            // Total load is still 35kN
            Assert.Equal(35.0,
                resultsWithAsymmetricLoad[reactionStartIndex + 1] +
                resultsWithAsymmetricLoad[reactionStartIndex + 2], 0.1);
        }
    }
}