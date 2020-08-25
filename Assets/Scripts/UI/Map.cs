using UnityEngine;
using UnityEngine.UI;

namespace A_Star.UI
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private GameObject background = null;
        [SerializeField] private Node nodePrefab = null;

        private int size;

        public void SetUp(int gridSize)
        {
            this.size = gridSize;

            var glg = GetComponent<GridLayoutGroup>();
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = gridSize;

            var backgroundWidth = background.GetComponent<RectTransform>().rect.width;
            glg.cellSize = new Vector2(backgroundWidth / gridSize, backgroundWidth / gridSize);

            /**
             * delete default children
             * @see https://answers.unity.com/questions/852866/getsiblingindex-is-not-returning-the-proper-index.html
             */
            foreach (Transform child in this.transform)
            {
                child.name = "__Destroy__";
                Destroy(child.gameObject);
            }
            transform.DetachChildren();

            for (int i = 0; i < gridSize * gridSize; i++)
            {
                var node = Instantiate(nodePrefab, this.transform);
                node.SetDefaultWhite();
            }
        }

        private enum NodeColor
        {
            DefaultWhite,
            Red,
            Green,
            Blue,
            Black,
        }

        public void DrawNode(Algorithm.Node node)
        {
            NodeColor nodeColor;

            switch (node.State)
            {
                case Algorithm.Node.NodeState.None:
                    nodeColor = NodeColor.DefaultWhite;
                    break;

                case Algorithm.Node.NodeState.Start:
                    nodeColor = NodeColor.Green;
                    break;

                case Algorithm.Node.NodeState.Destination:
                    nodeColor = NodeColor.Red;
                    break;

                case Algorithm.Node.NodeState.Finding:
                    nodeColor = NodeColor.Blue;
                    break;

                case Algorithm.Node.NodeState.Obstacle:
                    nodeColor = NodeColor.Black;
                    break;

                case Algorithm.Node.NodeState.ResultPath:
                    nodeColor = NodeColor.Green;
                    break;

                default:
                    nodeColor = NodeColor.DefaultWhite;
                    break;
            }

            drawNode(node, nodeColor);
        }

        /// <summary>
        /// 直角坐标系, 横坐标x, 纵坐标y, 左下角是(0, 0), 右上角是(size, size)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nodeColor"></param>
        /// <param name="num">在节点上画一个数字</param>
        private void drawNode(A_Star.Algorithm.Node nodeData, NodeColor nodeColor)
        {
            int x = nodeData.X;
            int y = nodeData.Y;

            // 将坐标系的值转化为grid的child的顺序
            int nodeIndex = (size - y - 1) * size + x;

            // out of range
            if (nodeIndex >= size * size || nodeIndex < 0) return;

            var node = transform.GetChild(nodeIndex).GetComponent<Node>();
            node.SetBaseCost(nodeData.BaseCost);
            node.SetCost(nodeData.Cost);
            switch (nodeColor)
            {
                case NodeColor.Red:
                    node.SetRed();
                    break;

                case NodeColor.Green:
                    node.SetGreen();
                    break;

                case NodeColor.Blue:
                    node.SetBlue();
                    break;

                case NodeColor.Black:
                    node.SetBlack();
                    break;

                case NodeColor.DefaultWhite:
                default:
                    node.SetDefaultWhite();
                    break;
            }
        }
    }
}