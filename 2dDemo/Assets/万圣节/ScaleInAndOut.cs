using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInAndOut : MonoBehaviour
{
    // Start is called before the first frame update
    public float scaleSize = 0.2f;
    float baseSize = 1.1f;
    void Start()
    {
        baseSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        var x = baseSize + Mathf.Sin(Time.realtimeSinceStartup) * scaleSize;
        transform.localScale = new Vector3(x, x, 0f);
    }
}
