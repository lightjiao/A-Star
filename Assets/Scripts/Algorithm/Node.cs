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
        private int i;
        private int j;

        public Node(NodeState state, int i, int j) : this()
        {
            State = state;
            this.i = i;
            this.j = j;
        }
    }
}