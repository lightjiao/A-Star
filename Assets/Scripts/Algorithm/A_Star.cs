using System.Collections;
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
        [Tooltip("地图类型")]
        [SerializeField]
        private MapGenerator.Type mapType = MapGenerator.Type.随机;

        [Tooltip("地图大小")]
        [SerializeField]
        private int mapSize = 16;

        //[Tooltip("帧数")]
        //[SerializeField]
        private float tickPerSecond;

        private float timeBetweenRedraw;

        private Map map;
        private HashSet<Node> openSet;
        private HashSet<Node> closeSet;

        // 当前正在运行的协程
        private Coroutine currentCoroutine = null;

        private void Start()
        {
            openSet = new HashSet<Node>();
            closeSet = new HashSet<Node>();

            StartAlgorithm();
        }

        /// <summary>
        /// 开始A*算法
        /// </summary>
        public void StartAlgorithm()
        {
            // 每秒遍历多少个节点
            tickPerSecond = mapSize * mapSize / 8;
            // 获取运行的帧数
            timeBetweenRedraw = 1 / tickPerSecond;

            map = MapGenerator.GenMap(mapType, mapSize);
            openSet.Clear();
            closeSet.Clear();

            startAlgorithmWrapper();
        }

        /// <summary>
        /// 控制当前运行的协程的wrapper
        /// </summary>
        private void startAlgorithmWrapper()
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            currentCoroutine = StartCoroutine(algorithm());
        }

        /// <summary>
        /// 算法动态展示的间隔时间
        /// </summary>
        /// <returns></returns>
        private IEnumerator interval()
        {
            yield return new WaitForSeconds(timeBetweenRedraw);
        }

        /// <summary>
        /// A* 算法本体
        /// </summary>
        /// <returns></returns>
        private IEnumerator algorithm()
        {
            var startNode = map.GetStartNode();
            startNode.BaseCost = 0;
            startNode.Cost = 0;
            addToOpenSet(startNode);

            while (openSet.Count > 0)
            {
                var node = selecInOpenSet();

                if (node.State == Node.NodeState.Destination)
                {
                    yield return calculateResultPath(node);
                    break;
                }
                else
                {
                    removeFromOpenSet(node);
                    yield return addToCloseSet(node);
                    foreach (Node aroundNode in getAroundNodes(node))
                    {
                        if (aroundNode.State == Node.NodeState.Obstacle) continue;

                        if (isInCloseSet(aroundNode)) continue;
                        if (isInOpenSet(aroundNode)) continue;

                        var newBaseCost = node.BaseCost + FromAtoB(node, aroundNode);
                        aroundNode.BaseCost = Mathf.Min(newBaseCost, aroundNode.BaseCost);
                        aroundNode.Cost = aroundNode.BaseCost + heuristicCost(aroundNode);
                        aroundNode.parent = node;

                        addToOpenSet(aroundNode);
                    }
                }
            }
        }

        private IEnumerable<Node> getAroundNodes(Node node)
        {
            int[] cordinateAdditions = { -1, 0, 1 };
            foreach (var xAddition in cordinateAdditions)
            {
                foreach (var yAddition in cordinateAdditions)
                {
                    if (xAddition == 0 && yAddition == 0) continue;
                    var newNode = map.GetNode(node.X + xAddition, node.Y + yAddition);
                    if (newNode != null)
                    {
                        yield return newNode;
                    }
                }
            }
        }

        /// <summary>
        /// 加入到已经遍历过的集合中
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerator addToCloseSet(Node node)
        {
            node.State = Node.NodeState.Finding;
            closeSet.Add(node);
            yield return interval();
        }

        private void removeFromOpenSet(Node node)
        {
            openSet.Remove(node);
        }

        private IEnumerator calculateResultPath(Node node)
        {
            while (node != null)
            {
                yield return interval();

                if (node.State != Node.NodeState.Destination)
                {
                    node.State = Node.NodeState.ResultPath;
                }

                node = node.parent;
            }
        }

        /// <summary>
        /// 加入到待遍历的集合
        /// </summary>
        /// <param name="node"></param>
        private void addToOpenSet(Node node)
        {
            openSet.Add(node);
        }

        /// <summary>
        /// 在OpenSet中选择cost最小的一个点
        /// </summary>
        /// <returns></returns>
        private Node selecInOpenSet()
        {
            float minCost = float.MaxValue;

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

        /// <summary>
        /// 顶点到终点的预期距离, h(n)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private float heuristicCost(Node node)
        {
            return 2.5f * AbsDistance(node, map.GetDestinationNode());
        }

        // 水平距离代价因子
        private static readonly int D = 1;
        // 对焦距离代价因子
        private static readonly float D2 = Mathf.Sqrt(2);

        /// <summary>
        /// 绝对的距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private float AbsDistance(Node a, Node b)
        {
            return Mathf.Abs(FromAtoB(a, b));
        }

        private float FromAtoB(Node a, Node b)
        {
            // 向量
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;

            // 向量长度
            float len = Mathf.Sqrt(dx * dx + dy * dy);

            // 向量的夹角是正还是负
            var minusFlag = 1;
            var sin = dy / len;


            // 水平方向的负数



            return D2 * Mathf.Min(dx, dy) + D * Mathf.Abs(dx - dy);
        }
    }
}