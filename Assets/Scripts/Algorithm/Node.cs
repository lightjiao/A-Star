using System.Collections;

namespace A_Star.Algorithm
{
    public class Node : IEqualityComparer
    {
        public enum NodeState
        {
            None,
            Start,
            Destination,
            Obstacle,
            Finding,
            ResultPath,
        }

        public NodeState State;
        public int X;
        public int Y;
        public int Cost;

        public Node parent;

        public Node(int i, int j, NodeState state = NodeState.None, int cost = 0)
        {
            this.X = i;
            this.Y = j;
            this.Cost = cost;
            this.State = state;
            this.parent = null;
        }

        bool IEqualityComparer.Equals(object a, object b)
        {
            Node nodeA = (Node)a;
            Node nodeB = (Node)b;
            return nodeA.X == nodeB.X && nodeA.Y == nodeB.Y;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            Node node = (Node)obj;
            int tmp = (node.Y + ((node.X + 1) / 2));
            return node.X + (tmp * tmp);
        }
    }
}