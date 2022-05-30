using UnityEngine;

public class RightMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        p.x += 0.01f;
        transform.position = p;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("卧槽，撞车了");
    }
}