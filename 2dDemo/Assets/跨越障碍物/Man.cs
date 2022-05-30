using UnityEngine;

public class Man : MonoBehaviour
{
    public Vector2 initSpeed;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GetComponent<Rigidbody2D>();
        obj.velocity = initSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        if (p.y < 0)
        {
            p.y = 0;
        }

        transform.position = p;
        var obj = GetComponent<Rigidbody2D>();
        var v = obj.velocity;
        v.x = initSpeed.x;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("加速了");
            v.y = Config.manSpeed; //产生一个向上的初速度
        }
        obj.velocity = v;
        transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("卧槽，撞车了");
        var obj = GetComponent<Rigidbody2D>();
        obj.velocity = Vector2.zero;
        obj.rotation = 0;
    }
}