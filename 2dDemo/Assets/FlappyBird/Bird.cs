using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace FlappyBird
{
    public class Bird : MonoBehaviour
    {
        private Main main;
        private Camera camera;
        private Land land;

        void Start()
        {
            main = FindObjectOfType<Main>();
            camera = FindObjectOfType<Camera>();
            land = FindObjectOfType<Land>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var bird = GetComponent<Rigidbody2D>();
                bird.AddForce(Vector2.up * main.force, ForceMode2D.Impulse);
                transform.rotation = quaternion.identity;
            }

            var birdPosition = transform.position;
            if (birdPosition.x < -1)
            {
                birdPosition.x = -1;
                transform.position = birdPosition;
            }

            // var p = camera.transform.position;
            // p.x = transform.position.x;
            // camera.transform.position = p;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            print("碰撞啦");
            main.pause();
        }
    }
}