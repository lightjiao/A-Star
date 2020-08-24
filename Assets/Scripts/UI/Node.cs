using UnityEngine;
using UnityEngine.UI;

namespace A_Star.UI
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Image image = null;
        [SerializeField] private Text text = null;

        public void SetNumber(float num)
        {
            text.text = num.ToString("f1");
        }

        public void SetBlue()
        {
            image.color = new Color32(0, 0, 255, 255);
        }

        public void SetGreen()
        {
            image.color = new Color32(0, 255, 0, 255);
        }

        public void SetRed()
        {
            image.color = new Color32(255, 0, 0, 255);
        }

        public void SetBlack()
        {
            image.color = new Color32(0, 0, 0, 255);
        }

        public void SetDefaultWhite()
        {
            image.color = new Color32(255, 255, 255, 255);
        }
    }
}