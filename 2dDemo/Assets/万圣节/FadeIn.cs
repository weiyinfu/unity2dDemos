using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float faceInDuration; //淡入的时间

    float used = 0;

    // Start is called before the first frame update
    void Start()
    {
        setAlpha(0);
    }

    void setAlpha(float alpha)
    {
        var render = GetComponent<SpriteRenderer>();
        var c = render.color;
        c.a = Mathf.Min(alpha, 1);
        render.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        if (faceInDuration == 0)
        {
            faceInDuration = 3;
        }
        used += Time.deltaTime;
        float alpha = used / faceInDuration;
        setAlpha(alpha);
    }
}