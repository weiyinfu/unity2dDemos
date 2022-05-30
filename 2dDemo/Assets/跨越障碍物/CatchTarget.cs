using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTarget : MonoBehaviour
{
    public GameObject target;

    public Vector2 shift;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var p = target.transform.position;
        p.x += shift[0];
        p.y =0;
        p.z = -10;
        transform.position = p;
    }
}