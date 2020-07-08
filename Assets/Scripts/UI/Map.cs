using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A_Star.UI
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private GameObject background = null;
        [SerializeField] private Node nodePrefab = null;

        private int gridSize;

        public void SetUp(int gridSize)
        {
            this.gridSize = gridSize;

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

        public void DrawNode(int x, int y, Algorithm.Node.NodeState nodeState)
        {
            NodeColor nodeColor;

            switch (nodeState)
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

            this.drawNode(x, y, nodeColor);
        }

        private void drawNode(int x, int y, NodeColor nodeColor)
        {
            int nodeIndex = x + y * gridSize;

            // out of range
            if (nodeIndex >= gridSize * gridSize || nodeIndex < 0) return;

            // print((x, y, nodeIndex));

            var node = transform.GetChild(nodeIndex).GetComponent<Node>();
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