﻿using System.Collections.Generic;
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

        private void Start()
        {
            generateObstacleData();
            initMap();

            gridUI.SetUp(gridSize);
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

        private bool isInTheMap(Node node)
        {
            if (node.X < 0 || node.Y < 0) return false;
            if (node.X >= gridSize || node.Y >= gridSize) return false;

            return true;
        }

        private void generateObstacleData()
        {
            int obstacleNumber = Mathf.Max(gridSize / 8, 3); // 至少3个障碍物
            maxObstacleLength = gridSize / 4;

            // 初始化几个obstacle的端点
            var headObstacleNodes = new List<Node>();
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