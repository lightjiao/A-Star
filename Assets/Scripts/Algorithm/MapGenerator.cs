using System;
using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    /// <summary>
    /// 生成地图数据
    /// </summary>
    public static class MapGenerator
    {
        /// <summary>
        /// 地图类型
        /// </summary>
        public enum Type
        {
            随机,
            口袋形,
            一字形,
            二字形,
            三字形,
        }

        static MapGenerator()
        {
            funcMap = new Dictionary<Type, GenMapDelegate>
            {
                [Type.随机] = GenRandom,
                [Type.口袋形] = GenThatMap,
                [Type.一字形] = (size) => { return GenWithHorizontalObstacle(1, size); },
                [Type.二字形] = (size) => { return GenWithHorizontalObstacle(2, size); },
                [Type.三字形] = (size) => { return GenWithHorizontalObstacle(3, size); },
            };
        }

        public static Map GenMap(Type type, int size)
        {
            return funcMap[type](size);
        }

        // 生成地图的函数委托
        private delegate Map GenMapDelegate(int size);

        // 生成地图的类型和对应的函数映射
        private static Dictionary<Type, GenMapDelegate> funcMap;

        /// <summary>
        /// 生成带横向障碍物的地图
        /// </summary>
        /// <param name="obstacleNum">横向障碍物的数量</param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static Map GenWithHorizontalObstacle(int obstacleNum, int size)
        {
            int x = size / (obstacleNum + 1);
            int obstacleLength = x;

            var obstacleHead = new List<Node>();
            for (int i = 1; i <= obstacleNum; i++)
            {
                obstacleHead.Add(new Node(x * i, x * i));
            }

            var obstacles = new List<Node>(obstacleHead);

            // 获取向左上角蔓延的节点
            Func<Node, List<Node>> GetUpLeftNode = (Node n) =>
            {
                var list = new List<Node>();
                list.Add(new Node(n.X - 1, n.Y));
                list.Add(new Node(n.X - 1, n.Y + 1));
                return list;
            };
            // 获取向右下角蔓延的节点
            Func<Node, List<Node>> GetDownRigheNode = (Node n) =>
            {
                var list = new List<Node>();
                list.Add(new Node(n.X + 1, n.Y));
                list.Add(new Node(n.X + 1, n.Y - 1));
                return list;
            };

            foreach (var node in obstacleHead)
            {
                var upLeftNode = node;
                var downRigheNode = node;
                for (int i = 0; i < obstacleLength / 2; i++)
                {
                    foreach (var newNode in GetUpLeftNode(upLeftNode))
                    {
                        obstacles.Add(newNode);
                        upLeftNode = newNode;
                    }

                    foreach (var newNode in GetDownRigheNode(downRigheNode))
                    {
                        obstacles.Add(newNode);
                        downRigheNode = newNode;
                    }
                }
            }

            return new Map(size, obstacles);
        }

        /// <summary>
        /// 生成一个巨字形的地图
        /// </summary>
        /// <returns></returns>
        public static Map GenThatMap(int size)
        {
            int minSize = 4;
            if (size < minSize)
            {
                throw new System.Exception($"生成`口袋形`地图时, size不能小于{minSize}");
            }

            // 默认假设起点是(0, 0), 终点是(size - 1, size - 1)
            // 起点和终点附近的两个端点
            int scale = size / 8;
            var nodeNearStart = new Node(2 * scale, 1 * scale);
            var nodeNearEnd = new Node(nodeNearStart.Y, size - nodeNearStart.X);

            var obstacles = new List<Node> { nodeNearStart, nodeNearEnd };
            // 横向填充
            for (int x = nodeNearStart.X; x < size - nodeNearStart.X; x++)
            {
                obstacles.Add(new Node(x, nodeNearStart.Y));
            }
            for (int x = nodeNearEnd.X; x < size - nodeNearStart.X; x++)
            {
                obstacles.Add(new Node(x, nodeNearEnd.Y));
            }
            // 纵向填充
            for (int y = nodeNearStart.Y; y <= nodeNearEnd.Y; y++)
            {
                obstacles.Add(new Node(size - nodeNearStart.X, y));
            }

            return new Map(size, obstacles);
        }

        /// <summary>
        /// 生成随机的地图
        /// </summary>
        /// <param name="size">地图大小</param>
        /// <returns></returns>
        public static Map GenRandom(int size = 16)
        {
            int obstacleNum = Mathf.Max(size / 4, 3); // 至少3个障碍物
            return GenRandom(size, obstacleNum);
        }

        /// <summary>
        /// 生成一个随机地图数据
        /// </summary>
        /// <param name="size">地图大小</param>
        /// <param name="obstacleNumber">地图中的障碍物数量</param>
        /// <returns></returns>
        public static Map GenRandom(int size, int obstacleNumber)
        {
            // 生成obstacle的端点
            var obstacleHead = new List<Node>();
            for (int i = 0; i < obstacleNumber; i++)
            {
                int randX = UnityEngine.Random.Range(0, size);
                int randY = UnityEngine.Random.Range(0, size);
                obstacleHead.Add(new Node(randX, randY));
            }

            // 障碍物的长度
            int obstacleLength = size / 4;

            // 根据obstacle端点生成连续的obstacles
            var obstacles = fillObstacle(obstacleHead, obstacleLength);

            return new Map(size, obstacles);
        }

        /// <summary>
        /// 填满障碍物
        /// </summary>
        /// <param name="obstaclesInit">障碍物起点</param>
        /// <param name="obstacleLength">障碍物长度</param>
        /// <returns></returns>
        private static List<Node> fillObstacle(List<Node> obstaclesInit, int obstacleLength)
        {
            var obstacles = new List<Node>(obstaclesInit);
            foreach (var node in obstaclesInit)
            {
                int newX = node.X;
                int newY = node.Y;

                int xAddition = 1;
                int yAddition = UnityEngine.Random.Range(-1, 2);
                for (int i = 0; i < obstacleLength; i++)
                {
                    newX += xAddition;
                    newY += yAddition;

                    if (yAddition != 0)
                    {
                        obstacles.Add(new Node(newX, newY - yAddition, Node.NodeState.Obstacle));
                    }

                    obstacles.Add(new Node(newX, newY, Node.NodeState.Obstacle));
                }
            }

            return obstacles;
        }
    }
}