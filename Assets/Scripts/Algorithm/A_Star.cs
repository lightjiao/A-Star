﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    /**
         * 初始化open_set和close_set；
         * 将起点加入open_set中，并设置优先级为0（优先级最高）；
         * 如果open_set不为空，则从open_set中选取优先级最高的节点n：
         * 如果节点n为终点，则：
                * 从终点开始逐步追踪parent节点，一直达到起点；
                * 返回找到的结果路径，算法结束；
            * 如果节点n不是终点，则：
                * 将节点n从open_set中删除，并加入close_set中；
                * 遍历节点n所有的邻近节点：
                    * 如果邻近节点m在close_set中，则：
                        * 跳过，选取下一个邻近节点
                    * 如果邻近节点m也不在open_set中，则：
                        * 设置节点m的parent为节点n
                        * 计算节点m的优先级
                        * 将节点m加入open_set中
         */
    public class A_Star : MonoBehaviour
    {
        HashSet<Node> openSet;
        HashSet<Node> closeSet;

        Map map = null;

        private void Start()
        {
            map = GetComponent<Map>();

            openSet = new HashSet<Node>();
            closeSet = new HashSet<Node>();
        }

        public void Algorithm()
        {
            map.Redraw();

            var startNode = map.GetStartNode();
            startNode.Cost = 0;
            addToOpenSet(startNode);

            while (openSet.Count > 0)
            {
                var node = selecInOpenSet();

                if (node.State == Node.NodeState.Destination)
                {
                    calculateResultPath();
                    break;
                }
                else
                {
                    removeFromOpenSet(node);
                    addToCloseSet(node);
                    foreach (Node aroundNode in getAroundNodes(node))
                    {
                        if (isInCloseSet(aroundNode)) continue;
                        if (isInOpenSet(aroundNode)) continue;

                        aroundNode.parent = node;
                        calculateCost(aroundNode);
                        addToOpenSet(aroundNode);
                    }
                }
            }

            map.Redraw();
        }

        private void calculateCost(Node node)
        {
            node.Cost = totalCost(node);
        }

        private IEnumerable<Node> getAroundNodes(Node node)
        {
            int[] cordinateAdditions = { -1, 0, 1 };
            foreach (var xAddition in cordinateAdditions)
            {
                foreach (var yAddition in cordinateAdditions)
                {
                    if (xAddition == 0 && yAddition == 0) continue;

                    yield return map.GetNode(node.X + xAddition, node.Y + yAddition);
                }
            }
        }

        private void addToCloseSet(Node node)
        {
            node.State = Node.NodeState.Finding;
            closeSet.Add(node);
        }

        private void removeFromOpenSet(Node node)
        {
            openSet.Remove(node);
        }

        private void calculateResultPath()
        {
            var node = map.GetDestinationNode();
            while (node != null)
            {
                node.State = Node.NodeState.ResultPath;

                node = node.parent;
            }
        }

        private void addToOpenSet(Node node)
        {
            openSet.Add(node);
        }

        private Node selecInOpenSet()
        {
            int minCost = int.MaxValue;
            Node resultNode = null;
            foreach (var node in openSet)
            {
                if (node.Cost < minCost)
                {
                    minCost = node.Cost;
                    resultNode = node;
                }
            }

            return resultNode;
        }

        private bool isInOpenSet(Node node)
        {
            return openSet.Contains(node);
        }

        private bool isInCloseSet(Node node)
        {
            return closeSet.Contains(node);
        }

        // g(n) 
        private int baseCost(Node node)
        {
            return distance(node, map.GetStartNode());
        }

        // h(n)
        private int heuristicCost(Node node)
        {
            return distance(node, map.GetStartNode());
        }

        // f(n) = g(n) + h(n)
        private int totalCost(Node node)
        {
            return baseCost(node) + heuristicCost(node);
        }

        private int distance(Node a, Node b)
        {
            int dx = Mathf.Abs(a.X - b.X);
            int dy = Mathf.Abs(a.Y - b.Y);

            return Mathf.Min(dx, dy) + Mathf.Abs(dx - dy);
        }

    }
}