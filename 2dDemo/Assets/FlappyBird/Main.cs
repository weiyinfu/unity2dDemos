using UnityEngine;

namespace FlappyBird
{
    public class Main : MonoBehaviour
    {
        public bool UseNight = false;
        public float force = 1f;
        public float landSpeed = .06f;
        public bool Pause = false;
        public float PipeHeight = 3.2f;
        public GameObject pipeUpPrefab;

        public GameObject pipeDownPrefab;
        private GameObject lastPipe;
        private float lastGeneratePipeTime=0;
        // Start is called before the first frame update
        void Start()
        {
            if (UseNight)
            {
            }
            else
            {
            }
        }

        public void pause()
        {
            this.Pause = true;
            foreach (var i in FindObjectsOfType<Pipe>())
            {
                i.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (lastPipe.transform.position.x < -0.5)
            {

            }
        }
    }
}