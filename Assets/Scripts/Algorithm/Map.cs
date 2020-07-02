using System.Collections.Generic;
using UnityEngine;

namespace A_Star.Algorithm
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private UI.Map uiGrid = null;

        // init map(2D vector)
        //      random obstcle
        //      start point & end point
        // A*

        private const int mapLength = 50;
        private List<List<NodeState>> map;

        public enum NodeState
        {
            None,
            Obstacle,
            Finding,
            Result,
        }

        private void Start()
        {
        }

        private void initMap()
        {
            map = new List<List<NodeState>>(50);
            for (int i = 0; i < mapLength; i++)
            {
                map[i] = new List<NodeState>(50);
            }

            initObstacle();
        }

        private void initObstacle()
        {
        }
    }
}