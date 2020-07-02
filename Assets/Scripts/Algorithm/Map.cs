using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private UI.Map uiGrid = null;

        private const int mapLength = 50;
        private List<List<Node>> map;

        private void Start()
        {
            initMap();
            initObstacle();
        }

        private void initMap()
        {
            map = new List<List<Node>>(mapLength);
            for (int i = 0; i < mapLength; i++)
            {
                map.Add(new List<Node>(mapLength));
                for (int j = 0; j < mapLength; j++)
                {
                    map[i].Add(new Node(Node.NodeState.None, i, j));
                }
            }
        }

        private void initObstacle()
        {
            // 
        }
    }
}