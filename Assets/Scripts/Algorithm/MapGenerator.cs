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
            巨字形,
        }

        static MapGenerator()
        {
            funcMap = new Dictionary<Type, GenMapDelegate>
            {
                [Type.随机] = GenRandom,
                [Type.巨字形] = GenThatMap
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
        /// 生成一个巨字形的地图
        /// </summary>
        /// <returns></returns>
        public static Map GenThatMap(int size)
        {
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
                int randX = Random.Range(0, size);
                int randY = Random.Range(0, size);
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
                int yAddition = Random.Range(-1, 2);
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