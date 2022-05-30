using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

/*
 * 2048第一版，这一版的缺点在于：先基于prefab初始化好每个位置的卡片，然后之后就只需要更改卡片上的数字。
 * 这样做的缺点就是没法模拟游戏物体的运动。
 */
namespace Game2048
{
    public class Haha : MonoBehaviour
    {
        // Start is called before the first frame update
        GameObject[][] a; //二维数组，存储游戏对象
        int[][] b; //二维数组，存储数值
        int[][] snap; //快照，用于diff
        public GameObject prefab; //游戏卡片的prefab
        private GameObject gameOverText; //游戏结束文本
        private GameObject scoreText; //分数文本
        private ColorUtil colorUtil;
        private Vector2Int lastGenerate;

        void Start()
        {
            colorUtil = new ColorUtil(30); //初始化每个颜色块的颜色
            gameOverText = GameObject.Find("GameOver");
            scoreText = GameObject.Find("Score");
            gameOverText.SetActive(false); //gameOver隐藏掉
            a = new GameObject[4][];
            b = new int[4][];
            for (int i = 0; i < 4; i++) b[i] = new int[4];
            for (int i = 0; i < 4; i++)
            {
                a[i] = new GameObject[4];
                for (int j = 0; j < 4; j++)
                {
                    var x = Instantiate(prefab);
                    x.name = "sphere" + (i * 4 + j);
                    a[i][j] = x;
                    x.transform.SetPositionAndRotation(new Vector3(j * 3 - 5, 5 - (i * 3), 0), Quaternion.identity);
                }
            }

            b[(int) (Random.value * 4)][(int) (Random.value * 4)] = 2;
            Snapshot();
            Render();
            UpdateGameScore();
        }

        void Render()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    TextMesh t = a[i][j].GetComponent<TextMesh>();
                    var sprite = a[i][j].GetComponentInChildren<SpriteRenderer>();
                    CardColor cardColor = GetColor(b[i][j]);
                    sprite.color = cardColor.back;
                    t.color = cardColor.fore;
                    //如果是刚刚生成的新块，则更改一下它的颜色
                    if (i == lastGenerate.x && j == lastGenerate.y)
                    {
                        t.color = Color.red;
                    }

                    if (b[i][j] > 0)
                    {
                        t.text = "" + b[i][j];
                    }
                    else
                    {
                        t.text = "";
                    }
                }
            }
        }

        private CardColor GetColor(int i)
        {
            if (i == 0)
            {
                return new CardColor(Color.grey, Color.white);
            }

            int index = Mathf.RoundToInt(Mathf.Log(i * 1.0f) / Mathf.Log(2.0f));
            if (index >= colorUtil.colorList.Count)
            {
                index = colorUtil.colorList.Count - 1;
            }

            return colorUtil.colorList[index];
        }

        void Up()
        {
            Debug.Log("Up");
            Rotate();
            Rotate();
            Down();
            Rotate();
            Rotate();
        }

        void Down()
        {
            Debug.Log("Down");

            for (int i = 0; i < 4; i++)
            {
                List<int> v = new List<int>();
                for (int j = 3; j >= 0; j--)
                {
                    if (b[j][i] > 0)
                    {
                        v.Add(b[j][i]);
                    }
                }

                List<int> vv = new List<int>();
                for (int j = 0; j < v.Count; j++)
                {
                    if (vv.Count > 0 && vv[vv.Count - 1] == v[j])
                    {
                        vv[vv.Count - 1] *= 2;
                        j++;
                        while (j < v.Count)
                        {
                            vv.Add(v[j]);
                            j++;
                        }
                    }
                    else
                    {
                        vv.Add(v[j]);
                    }
                }

                Debug.Log("vvCount" + vv.Count);
                for (int j = 0; j < vv.Count; j++)
                {
                    b[3 - j][i] = vv[j];
                }

                for (var j = vv.Count; j < 4; j++)
                {
                    b[3 - j][i] = 0;
                }
            }
        }

        void Rotate()
        {
            //旋转90度
            var c = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                c[i] = new int[4];
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    c[i][j] = b[j][3 - i];
                }
            }

            b = c;
        }

        void Left()
        {
            Debug.Log("Left");
            Rotate();
            Down();
            Rotate();
            Rotate();
            Rotate();
        }

        void Right()
        {
            Debug.Log("Right");
            Rotate();
            Rotate();
            Rotate();
            Down();
            Rotate();
        }

        void Generate()
        {
            //在空白位置处生成2或者4
            List<Vector2Int> spaces = new List<Vector2Int>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (b[i][j] == 0)
                    {
                        spaces.Add(new Vector2Int(i, j));
                    }
                }
            }

            if (spaces.Count == 0)
            {
                return;
            }

            var ind = (int) (Random.value * spaces.Count);
            this.lastGenerate = spaces[ind];
            b[lastGenerate.x][lastGenerate.y] = 2;
        }

        void Snapshot()
        {
            //执行镜像操作，把数据记录到snap数组里面，用于判断是否发生了合并
            if (snap == null)
            {
                snap = new int[4][];
                for (int i = 0; i < 4; i++) snap[i] = new int[4];
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    snap[i][j] = b[i][j];
                }
            }
        }

        bool HasChange()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (snap[i][j] != b[i][j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Update is called once per frame
        void Update()
        {
            var keydown = false;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                keydown = true;
                Up();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                keydown = true;
                Down();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                keydown = true;
                Left();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                keydown = true;
                Right();
            }

            if (keydown)
            {
                if (HasChange())
                {
                    Generate();
                    Render();
                    Snapshot();
                    UpdateGameScore();
                }
                else
                {
                    if (CheckOver())
                    {
                        gameOverText.SetActive(true);
                    }

                    Debug.Log("not HasChange");
                }
            }
        }

        private void UpdateGameScore()
        {
            int s = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (b[i][j] != 0)
                    {
                        s += Mathf.RoundToInt(Mathf.Pow(3, Mathf.Log(b[i][j], 2)));
                    }
                }
            }

            scoreText.GetComponent<Text>().text = "Score: " + s;
        }

        private bool CheckOver()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (b[i][j] == b[i][j - 1])
                    {
                        return false;
                    }
                }
            }

            for (int i = 1; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (b[i][j] == b[i - 1][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}