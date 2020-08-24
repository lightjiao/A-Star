using System.Collections;

namespace A_Star.Algorithm
{
    public class Node : IEqualityComparer
    {
        public Map.drawNodeDelegate DrawUI;

        public enum NodeState
        {
            None,
            Start,
            Destination,
            Obstacle,
            Finding,
            ResultPath,
        }

        private NodeState _state;

        public NodeState State
        {
            get { return _state; }
            set
            {
                _state = value;
                DrawUI(this);
            }
        }

        public int X;
        public int Y;

        /// <summary>
        /// 总cost
        /// </summary>
        public float Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                DrawUI(this);
            }
        }

        private float _cost = 0;

        /// <summary>
        /// 节点距离起点的cost
        /// </summary>
        public float BaseCost
        {
            get { return _baseCost; }
            set { _baseCost = value; }
        }

        private float _baseCost = 0;

        // 用于回溯路径时的记录
        public Node parent = null;

        public Node(int i, int j, NodeState state = NodeState.None)
        {
            this.X = i;
            this.Y = j;
            this._state = state;
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