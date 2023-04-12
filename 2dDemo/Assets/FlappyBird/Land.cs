using UnityEngine;

namespace FlappyBird
{
    public class Land : MonoBehaviour
    {
        // Start is called before the first frame update
        private Main main;

        void Start()
        {
            main = FindObjectOfType<Main>();
        }

        // Update is called once per frame
        void Update()
        {
            if (main.Pause) return;
            var m = GetComponent<SpriteRenderer>().material;
            var p = m.mainTextureOffset;
            p.x += main.landSpeed * Time.deltaTime;
            m.mainTextureOffset = p;
        }
    }
}