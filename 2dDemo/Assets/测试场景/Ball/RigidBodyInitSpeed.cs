using UnityEngine;

public class RigidBodyInitSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    public float initSpeedX;
    public float initSpeedY;

    void Start()
    {
        var obj = GetComponent<Rigidbody2D>();
        obj.velocity = new Vector2(initSpeedX, initSpeedY);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogFormat("v={0} p={1}", GetComponent<Rigidbody2D>().velocity, transform.position);
    }
}