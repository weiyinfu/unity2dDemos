using UnityEngine;
using UnityEngine.UI;

namespace Computer
{
    public class Main : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var can = GameObject.Find("Canvas");
            int sz = 50;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var b = new GameObject();
                    b.AddComponent<Button>();
                    b.AddComponent<Text>();
                    b.GetComponent<Button>().image = Resources.Load<Image>("UISprite");
                    b.GetComponent<Text>().text = "" + (i * 3 + j);
                    b.name = "Button" + i + "-" + j;
                    b.transform.position = new Vector3(i * sz, j * sz);
                    b.transform.SetParent(can.transform);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}