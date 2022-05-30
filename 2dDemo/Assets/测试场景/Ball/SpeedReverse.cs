using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReverse : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 v;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        v = GetComponent<Rigidbody2D>().velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().velocity=-v;
    }
}
