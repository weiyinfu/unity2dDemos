using System.Collections;
using UnityEngine;

namespace UseCoroutine
{
    public class Main : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            print("start is called");
            StartCoroutine("go");
            print("start over");
        }

        // Update is called once per frame
        void Update()
        {
        }

        public IEnumerator go()
        {
            print("go is called");
            print(Time.frameCount);
            yield return "one";
            print(Time.frameCount);
            yield return "two";
            print(Time.frameCount);
            yield return "three";
        }
    }
}