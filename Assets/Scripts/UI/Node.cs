using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace A_Star.UI
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Image image = null;

        public void SetBlue()
        {
            image.color = new Color(0, 0, 100);
        }

        public void SetGreen()
        {
            image.color = new Color(0, 100, 0);
        }

        public void SetRed()
        {
            image.color = new Color(50, 0, 0);
        }

        public void SetBlack()
        {
            image.color = new Color(0, 0, 0);
        }

        public void SetDefaultWhite()
        {
            image.color = new Color(200, 200, 200);
        }
    }
}