using UnityEngine;

public class OutDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (cam.transform.position.x - transform.position.x > Config.disappearDistance)
        {
            Destroy(this.gameObject);
            Debug.Log("deleted bar " + this.name);
        }
    }
}