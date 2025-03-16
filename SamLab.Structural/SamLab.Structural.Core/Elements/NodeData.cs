using System.Collections.Generic;
using System.Numerics;
using SamLab.Structural.Core.Elements.utilities;

namespace SamLab.Structural.Core.Elements
{
    public struct NodeData
    {
        public Vector2 Position { get; private set; }
        public int Id { get; set; }

        public Vector2 AppliedForce;
        public Vector2 Displacement;

        public List<MemeberData> Members;
        //public List<(int MemberId, int ConnectedNodeId)> ConnectedMembers;

        public Support Support { get; private set; }
        public NodeData(Vector2 position)
        {
            Position = position;
            Id = GlobalIdHandler.GetNextNodeIndex(); 
            AppliedForce = Vector2.Zero;
            Displacement = Vector2.Zero;
            Members = null;
            Support = null;
        }

        public void SetForce(Vector2 force)
        {
            AppliedForce = force;
        }

        public void SetPosition(Vector2 position)
        {
            Displacement = position - Position;
            Position = position;
        }

        public void Reset()
        {
            AppliedForce = Vector2.Zero;
            Displacement = Vector2.Zero;
        }

        public void AddMember(MemeberData memeberData)
        {
            if(Members.Contains(memeberData))
                return;

            Members.Add(memeberData);
        }

        public void RemoveMember(MemeberData memeberData)
        {
            if (!Members.Contains(memeberData))
                return;
            Members.Remove(memeberData);
        }

        public void AddSupprot(Support support)
        {
            Support = support;
        }
    }
}
