﻿using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private UI.Map gridUI = null;
        private const int gridSize = 50;
        private List<List<Node>> gridData;

        private int obstacleNumber = gridSize / 8;
        private List<(int x, int y)> obstacleList;
        private int minObstacleLength = 3;
        private int maxObstacleLength = gridSize / 4;

        private void Start()
        {
            initMap();
            initObstacle();
        }

        private void initMap()
        {
            gridData = new List<List<Node>>(gridSize);
            for (int i = 0; i < gridSize; i++)
            {
                gridData.Add(new List<Node>(gridSize));
                for (int j = 0; j < gridSize; j++)
                {
                    gridData[i].Add(new Node(Node.NodeState.None, i, j));
                }
            }
        }

        private void initObstacle()
        {
            obstacleList = new List<(int x, int y)>();

            // 初始化几个obstacle的端点
            obstacleList.Add((gridSize / 2, gridSize / 2));
            for (int i = 0; i < obstacleNumber - 1; i++)
            {
                int randX = Random.Range(0, gridSize);
                int randY = Random.Range(0, gridSize);
                obstacleList.Add((randX, randY));
            }

            // 根据obstacle端点生成连续的obstacles
            foreach (var (x, y) in obstacleList)
            {
                int newX = x;
                int newY = y;

                int xAddition = 1;
                int yAddition = Random.Range(-1, 2);
                int obstacleLength = Random.Range(minObstacleLength, maxObstacleLength);
                for (int i = 0; i < obstacleLength; i++)
                {
                    newX += xAddition;
                    newY += yAddition;
                    obstacleList.Add((newX, newY));
                }
            }
        }
    }
}