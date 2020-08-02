using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    public class Map : MonoBehaviour
    {
        // UI
        [SerializeField] private UI.Map gridUI = null;

        // Map Data
        [SerializeField] private int gridSize = 16;

        private List<List<Node>> gridMapData;

        // Obstacles
        private List<Node> obstacleNodes;

        private int minObstacleLength = 3;
        private int maxObstacleLength;

        public void SetUp()
        {
            generateObstacleData();
            initMap();

            gridUI.SetUp(gridSize);
            Redraw();
        }

        public void Clear()
        {
            generateObstacleData();
            initMap();

            Redraw();
        }

        // PUBLIC

        public Node GetStartNode()
        {
            return GetNode(0, 0);
        }

        public Node GetDestinationNode()
        {
            return GetNode(gridSize - 1, gridSize - 1);
        }

        public Node GetNode(int x, int y)
        {
            if (false == isInTheMap(x, y)) return null;

            return gridMapData[x][y];
        }

        public bool IsValidNode(Node node)
        {
            return isInTheMap(node) && false == isObstacle(node);
        }

        public bool IsDestination(Node node)
        {
            return node.State == Node.NodeState.Destination;
        }

        public bool IsStart(Node node)
        {
            return node.State == Node.NodeState.Start;
        }

        public void Redraw()
        {
            foreach (var gridColumn in gridMapData)
            {
                foreach (var node in gridColumn)
                {
                    gridUI.DrawNode(node.X, node.Y, node.State);
                }
            }
        }

        // PRIVATE

        private bool isObstacle(Node node)
        {
            return node.State == Node.NodeState.Obstacle;
        }

        private bool isInTheMap(int x, int y)
        {
            if (x < 0 || y < 0) return false;
            if (x >= gridSize || y >= gridSize) return false;

            return true;
        }

        private bool isInTheMap(Node node)
        {
            return isInTheMap(node.X, node.Y);
        }

        private void generateObstacleData()
        {
            int obstacleNumber = Mathf.Max(gridSize / 4, 3); // 至少3个障碍物
            maxObstacleLength = gridSize / 4;

            // 初始化几个obstacle的端点
            var headObstacleNodes = new List<Node>();
            for (int i = 0; i < obstacleNumber; i++)
            {
                int randX = Random.Range(0, gridSize);
                int randY = Random.Range(0, gridSize);
                headObstacleNodes.Add(new Node(randX, randY));
            }

            obstacleNodes = new List<Node>(headObstacleNodes);

            // 中间斜插着一条障碍物
            for (int i = gridSize * 3 / 8; i < gridSize * 5 / 8; i++)
            {
                for (int j = gridSize * 5 / 8; j >= gridSize * 3 / 8; j--)
                {
                    if (i + j != gridSize) continue;
                    obstacleNodes.Add(new Node(i, j, Node.NodeState.Obstacle));
                    obstacleNodes.Add(new Node(i, j + 1, Node.NodeState.Obstacle));
                }
            }

            // 根据obstacle端点生成连续的obstacles
            _generateObstacleNodes(headObstacleNodes);
        }

        private void _generateObstacleNodes(List<Node> headObstacleNodes)
        {
            foreach (var node in headObstacleNodes)
            {
                int newX = node.X;
                int newY = node.Y;

                int xAddition = 1;
                int yAddition = Random.Range(-1, 2);
                int obstacleLength = Random.Range(minObstacleLength, maxObstacleLength);
                for (int i = 0; i < obstacleLength; i++)
                {
                    newX += xAddition;
                    newY += yAddition;

                    if (yAddition != 0)
                    {
                        obstacleNodes.Add(new Node(newX, newY - yAddition, Node.NodeState.Obstacle));
                    }

                    obstacleNodes.Add(new Node(newX, newY, Node.NodeState.Obstacle));
                }
            }
        }

        private void initMap()
        {
            gridMapData = new List<List<Node>>(gridSize);
            for (int i = 0; i < gridSize; i++)
            {
                gridMapData.Add(new List<Node>(gridSize));
                for (int j = 0; j < gridSize; j++)
                {
                    gridMapData[i].Add(new Node(i, j));
                }
            }

            // init the start & desitination data
            gridMapData[0][0].State = Node.NodeState.Start;
            gridMapData[gridSize - 1][gridSize - 1].State = Node.NodeState.Destination;

            // init obstacle data
            foreach (var node in obstacleNodes)
            {
                if (false == isInTheMap(node)) continue;
                gridMapData[node.X][node.Y].State = node.State;
            }
        }
    }
}