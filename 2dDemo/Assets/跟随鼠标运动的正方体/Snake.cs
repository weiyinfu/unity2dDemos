using System;
using UnityEngine;

namespace 跟随鼠标
{
    public class Snake : MonoBehaviour
    {
        public float snakeSpeed = 0.5f;
        private Camera mainCamera;
        private GameObject snakeHead;

        private void Start()
        {
            mainCamera = FindObjectOfType<Camera>();
            snakeHead = GameObject.Find("Square");
        }

        void Update()
        {
            var p = Input.mousePosition;
            p = mainCamera.ScreenToWorldPoint(p);
            var head = snakeHead.transform.position;
            p.z = head.z;
            var ratio = snakeSpeed * Time.deltaTime / Vector3.Distance(head, p);
            ratio = Mathf.Min(ratio, 1);
            var nextPosition = Vector3.Lerp(head, p, ratio);
            snakeHead.transform.position = nextPosition;
        }
    }
}