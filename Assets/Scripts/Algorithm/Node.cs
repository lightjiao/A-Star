using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    public struct Node
    {
        public enum NodeState
        {
            None,
            Obstacle,
            Finding,
            Result,
        }

        public NodeState State;
        public int X;
        public int Y;

        public Node(int i, int j, NodeState state = NodeState.None)
        {
            this.X = i;
            this.Y = j;
            this.State = state;
        }
    }
}