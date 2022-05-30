using UnityEngine;

public class GenerateBar : MonoBehaviour
{
    // Start is called before the first frame update
    private float lastBar = 0;
    public GameObject barPrefab;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        if (p.x - lastBar > 5 + Random.value * 3)
        {
            //如果距离上一个bar太远了，则生成一个bar
            var bar = Instantiate(barPrefab);
            bar.name = "bar" + p.x;
            bar.transform.position = new Vector3(p.x + 6.0f + Random.value * 2, bar.transform.position.y, 0f);
            lastBar = p.x;
        }
    }
}