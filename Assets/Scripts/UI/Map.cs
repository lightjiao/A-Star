using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace A_Star.UI
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Node nodePrefab = null;

        private const int nodeNumber = 2500; // 50*50;

        // Start is called before the first frame update
        private void Start()
        {
            // delete default children
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < nodeNumber; i++)
            {
                var node = Instantiate(nodePrefab, this.transform);
                node.SetDefaultWhite();
            }
        }

        //public void ReDraw(List<List<NodeState>> map)
        //{
        //}
    }
}