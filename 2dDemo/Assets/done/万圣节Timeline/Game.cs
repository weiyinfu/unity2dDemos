using UnityEngine;
using UnityEngine.InputSystem;

namespace 万圣节2
{
    public class Game : MonoBehaviour
    {
        public GameObject words;
        private bool canClickWords = false;

        public void SetCanClick()
        {
            this.canClickWords = true;
            Debug.Log("Got Signal Can click");
        }

        private void Update()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                if (canClickWords)
                {
                    words.SetActive(true);
                }
            }
        }
    }
}