using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace 相机
{
    public class NewBehaviourScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Camera c = GetComponent<Camera>();
            print($@"
screenWidth={Screen.width},screenHeight={Screen.height}
aspect={c.aspect}
size={c.orthographicSize}
isOrthorgraphic={c.orthographic}
");
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}