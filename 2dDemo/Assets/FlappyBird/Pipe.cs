using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    public class Pipe : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var m = FindObjectOfType<Main>();
            var b = GetComponent<Rigidbody2D>();
            b.velocity = m.landSpeed * Vector2.left;
        }

        // Update is called once per frame
        void Update()
        {
            //移出界外之后自动销毁
            var p = transform.position;
            if (p.x < -5f)
            {
                Destroy(this);
            }
        }
    }
}