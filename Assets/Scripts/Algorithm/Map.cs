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
        private List<List<Node>> gridData;

        // Obstacles
        private List<Node> obstacleNodes;
        private int minObstacleLength = 3;
        private int maxObstacleLength;

        // Start & Destination
        private Node start;
        private Node destination;

        private void Start()
        {
            initMap();
            initObstacle();
            initStartAndDestination();
        }

        public bool IsObstacle(Node node)
        {
            foreach (var obstacle in obstacleNodes)
            {
                if (node.X == obstacle.X && node.Y == obstacle.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsInTheMap(Node node)
        {
            if (node.X < 0 || node.Y < 0) return false;
            if (node.X >= gridSize || node.Y >= gridSize) return false;

            return true;
        }

        public bool IsDestination(Node node)
        {
            return node.X == start.X && node.Y == start.Y;
        }

        private void initMap()
        {
            gridData = new List<List<Node>>(gridSize);
            for (int i = 0; i < gridSize; i++)
            {
                gridData.Add(new List<Node>(gridSize));
                for (int j = 0; j < gridSize; j++)
                {
                    gridData[i].Add(new Node(i, j));
                }
            }

            gridUI.SetUp(gridSize);
        }

        private void initObstacle()
        {
            var headObstacleNodes = new List<Node>();
            int obstacleNumber = Mathf.Max(gridSize / 8, 3); // 至少3个障碍物
            maxObstacleLength = gridSize / 4;

            // 初始化几个obstacle的端点
            headObstacleNodes.Add(new Node(gridSize / 2, gridSize / 2));
            for (int i = 0; i < obstacleNumber - 1; i++)
            {
                int randX = Random.Range(0, gridSize);
                int randY = Random.Range(0, gridSize);
                headObstacleNodes.Add(new Node(randX, randY));
            }

            // 根据obstacle端点生成连续的obstacles
            obstacleNodes = new List<Node>(headObstacleNodes);
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
                        obstacleNodes.Add(new Node(newX, newY - yAddition));
                    }

                    obstacleNodes.Add(new Node(newX, newY));
                }
            }

            // 绘制obstacle
            foreach (var node in obstacleNodes)
            {
                gridUI.DrawNode(node.X, node.Y, UI.Map.NodeColor.Black);
            }
        }

        private void initStartAndDestination()
        {
            start = new Node(0, 0);
            destination = new Node(gridSize - 1, gridSize - 1);

            gridUI.DrawNode(start.X, start.Y, UI.Map.NodeColor.Green);
            gridUI.DrawNode(destination.X, destination.Y, UI.Map.NodeColor.Red);
        }
    }
}