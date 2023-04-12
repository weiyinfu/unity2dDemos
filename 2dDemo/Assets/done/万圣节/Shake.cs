using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.1f;//蝙蝠飞翔的速度
    Vector3 pos;
    void Start()
    {
        this.pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //获取照相机的方法
        if (Time.frameCount % 15 == 1)
        {
            var direction = Random.value;
            var r = Random.Range(0, speed);
            var x = pos.x + r * Mathf.Cos(direction);
            var y = pos.y + r * Mathf.Sin(direction);
            transform.SetPositionAndRotation(new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
