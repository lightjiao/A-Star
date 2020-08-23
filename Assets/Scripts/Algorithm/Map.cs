using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    /// <summary>
    /// 逻辑层面的地图
    /// </summary>
    public class Map
    {
        // 当节点状态改变时, 提供回调的委托
        public delegate void drawNodeDelegate(Node node);

        private UI.Map mapUI = null;

        /// <summary>
        /// 地图大小
        /// </summary>
        private int size = 16;

        /// <summary>
        /// 地图数据
        /// </summary>
        private List<List<Node>> data;

        public Map(int size, List<Node> obstacles = null)
        {
            this.size = size;
            // 初始化UI
            initUI();

            // 初始化地图数据
            data = new List<List<Node>>(size);
            for (int i = 0; i < size; i++)
            {
                data.Add(new List<Node>(size));
                for (int j = 0; j < size; j++)
                {
                    var node = new Node(i, j)
                    {
                        DrawUI = DrawUI_Node
                    };
                    data[i].Add(node);
                }
            }

            // init the start & desitination data
            data[0][0].State = Node.NodeState.Start;
            data[size - 1][size - 1].State = Node.NodeState.Destination;

            // init obstacle data
            foreach (var node in obstacles)
            {
                if (false == isInTheMap(node)) continue;
                data[node.X][node.Y].State = node.State;
            }
        }

        // PUBLIC

        public void DrawUI_Node(Node node)
        {
            mapUI.DrawNode(node.X, node.Y, node.State);
        }

        public Node GetStartNode()
        {
            return GetNode(0, 0);
        }

        public Node GetDestinationNode()
        {
            return GetNode(size - 1, size - 1);
        }

        public Node GetNode(int x, int y)
        {
            if (false == isInTheMap(x, y)) return null;

            return data[x][y];
        }

        // PRIVATE

        /// <summary>
        /// 关联以及初始化UI
        /// </summary>
        private void initUI()
        {
            if (mapUI is null)
            {
                mapUI = Object.FindObjectOfType<UI.Map>();
            }

            mapUI.SetUp(size);
        }

        private bool isInTheMap(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            if (x >= size || y >= size) return false;

            return true;
        }

        private bool isInTheMap(Node node)
        {
            return isInTheMap(node.X, node.Y);
        }
    }
}