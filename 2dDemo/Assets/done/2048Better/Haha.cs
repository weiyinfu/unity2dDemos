using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Util;

namespace Game2048Better
{
    class Card : MonoBehaviour
    {
        public Haha G;
        public int value;
        public int nextValue;
        public Vector3 defaultScale;
        public Card merged; //已经合并了的卡片

        public void moveTo(Vector3 pos)
        {
            if (transform.position.Equals(pos)) return;
            StartCoroutine("MoveCoroutine", pos);
        }

        public void generateGrow(Vector3 scale)
        {
            StartCoroutine("generateGrowCoroutine", scale);
        }

        public void mergeGrow()
        {
            //发生合并的时候，先变大再变小
            StartCoroutine("mergeGrowCoroutine");
        }

        private IEnumerator mergeGrowCoroutine()
        {
            yield return AnimateUtil.growAndShrink(gameObject, defaultScale * 1.2f, defaultScale, 0.4f, 25);
        }

        private IEnumerator generateGrowCoroutine(Vector3 scale)
        {
            yield return new WaitForSeconds(0.4f);
            yield return AnimateUtil.scaleTo(gameObject, scale, 0.4f, 25);
        }

        private IEnumerator MoveCoroutine(Vector3 pos)
        {
            yield return AnimateUtil.moveToByLocalPosition(gameObject, pos, 0.4f, 25);
        }

        public void moveToAndDestroy(Vector3 des)
        {
            StartCoroutine("moveToAndDestroyCoroutine", des);
        }

        private IEnumerator moveToAndDestroyCoroutine(Vector3 p)
        {
            yield return MoveCoroutine(p);
            Destroy(gameObject);
        }
    }

    class Placeholder : MonoBehaviour
    {
    }

    public class Haha : MonoBehaviour
    {
        public GameObject prefabCard; //游戏卡片的prefab
        public GameObject prefabPlaceholder; //占位符prefab

        public int targetValue = 2048;
        Card[] a; //二维数组，存储游戏对象
        private Card[] snap; //快照，用于比较数据是否发生变化

        //UI对象
        private GameObject gameOverPanel; //游戏结束文本
        private GameObject scoreText; //分数文本
        private RectTransform gameContent;

        //内部数据
        private ColorUtil colorUtil;
        private Vector3[,] points;


        //audio
        public AudioSource AudioStart;
        public AudioSource AudioWin;
        public AudioSource AudioMove;
        public AudioSource AudioLose;
        private Text gameOverText;

        private Vector2Int[] directions = {Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up,};

        void Start()
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
            gameOverText = GameObject.Find("GameOverPanel/GameOver").GetComponent<Text>();
            scoreText = GameObject.Find("Score");
            gameOverPanel.GetComponent<Button>().onClick.AddListener(newGame);
            gameContent = GameObject.Find("GameContent").GetComponent<RectTransform>();


            colorUtil = new ColorUtil(30); //初始化每个颜色块的颜色
            points = Util.GridManager.getGridOfRectTransform(gameContent, 4, 4);
            //构建占位符，占位符组成一个网格
            for (var i = 0; i < 16; i++)
            {
                var obj = Instantiate(prefabPlaceholder, gameContent.gameObject.transform, true);
                var placeholder = obj.AddComponent<Placeholder>();
                var objRect = obj.GetComponent<RectTransform>().rect;
                obj.name = $"Placeholder{i / 4}-{i % 4}";
                var pos = points[i / 4, i % 4];
                obj.transform.localPosition = pos;
                obj.transform.localScale = new Vector3((gameContent.rect.width / 4) / objRect.width * 0.9f, (gameContent.rect.height / 4) / objRect.height * 0.9f);
            }

            newGame();
        }

        void newGame()
        {
            gameOverPanel.SetActive(false); //gameOver隐藏掉
            var candidates = FindObjectsOfType<Card>();
            foreach (var i in candidates)
            {
                Destroy(i.gameObject);
            }

            a = new Card[16];
            Generate();
            Snapshot();
            UpdateGameScore();
            AudioStart.Play();
        }

        Collection<int> getSpaceIndex()
        {
            var candidates = new Collection<int>();
            for (var i = 0; i < 16; i++)
            {
                if (a[i] == null)
                {
                    candidates.Add(i);
                }
            }

            return candidates;
        }

        void Generate()
        {
            var candidates = getSpaceIndex();
            if (candidates.Count == 0)
            {
                print($"cannot generate for no space anymore");
                return;
            }

            var obj = Instantiate(prefabCard, gameContent.transform, true);
            var card = obj.AddComponent<Card>();
            card.G = this;
            card.value = 2;
            if (Random.value < 1 / 4.0f)
            {
                card.value = 4;
            }

            card.name = "Card";
            var objRect = obj.GetComponent<RectTransform>().rect;
            var scale = new Vector3((gameContent.rect.width / 4) / objRect.width * 0.9f, (gameContent.rect.height / 4) / objRect.height * 0.9f);
            obj.transform.localScale = Vector3.zero;
            card.defaultScale = scale;
            card.generateGrow(scale);
            // obj.transform.localScale = scale;
            var ind = candidates[Random.Range(0, candidates.Count)];
            a[ind] = card;
            card.transform.localPosition = points[ind / 4, ind % 4];
            setCardPosition(card, ind);
            setCardColorAndText(card);
        }

        void setCardPosition(Card card, int position)
        {
            card.moveTo(points[position / 4, position % 4]);
        }

        void setCardColorAndText(Card card)
        {
            //设置背景色
            var cardColor = GetColor(card.value);
            TextMesh t = card.GetComponent<TextMesh>();
            var sprite = card.GetComponentInChildren<SpriteRenderer>();
            t.color = cardColor.fore;
            sprite.color = cardColor.back;
            t.text = card.value + "";
        }

        public CardColor GetColor(int i)
        {
            int index = Mathf.RoundToInt(Mathf.Log(i * 1.0f) / Mathf.Log(2.0f));
            if (index >= colorUtil.colorList.Count)
            {
                index = colorUtil.colorList.Count - 1;
            }

            return colorUtil.colorList[index];
        }

        private void UpdateGameScore()
        {
            int s = 0;
            foreach (var i in a)
            {
                if (i != null)
                {
                    s += Mathf.RoundToInt(Mathf.Pow(3, Mathf.Log(i.value, 2)));
                }
            }

            scoreText.GetComponent<Text>().text = "Score: " + s;
        }

        int Pos(int x, int y)
        {
            return x * 4 + y;
        }

        bool legal(int x, int y)
        {
            return x >= 0 && y >= 0 && x < 4 && y < 4;
        }

        bool checkWin()
        {
            foreach (var i in a)
            {
                if (i != null && i.value == targetValue)
                {
                    return true;
                }
            }

            return false;
        }

        private bool checkLose()
        {
            foreach (var i in a)
            {
                if (i == null)
                {
                    return false;
                }
            }

            for (var i = 0; i < 16; i++)
            {
                var (x, y) = (i / 4, i % 4);
                foreach (var d in directions)
                {
                    var (xx, yy) = (x + d.x, y + d.y);
                    if (legal(xx, yy))
                    {
                        var p = Pos(xx, yy);
                        if (a[p].value == a[i].value)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        string show()
        {
            StringBuilder o = new StringBuilder();
            var ind = 0;
            foreach (var i in a)
            {
                if (i == null)
                {
                    o.Append("0,");
                }
                else
                {
                    o.Append($"{i.value},");
                }

                ind++;
                if (ind % 4 == 0)
                {
                    o.Append('\n');
                }
            }

            return o.ToString();
        }

        void Up()
        {
            Rotate();
            Rotate();
            Down();
            Rotate();
            Rotate();
        }


        void Down()
        {
            for (int c = 0; c < 4; c++)
            {
                List<Card> v = new List<Card>();
                for (int r = 3; r >= 0; r--)
                {
                    var p = Pos(r, c);
                    if (a[p] != null)
                    {
                        v.Add(a[p]);
                    }
                }

                List<Card> vv = new List<Card>();
                var merged = false;
                for (int j = 0; j < v.Count; j++)
                {
                    if (!merged && vv.Count > 0 && vv[vv.Count - 1].value == v[j].value)
                    {
                        merged = true;
                        var card = vv[vv.Count - 1];
                        card.nextValue = v[j].value * 2;
                        card.merged = v[j];
                    }
                    else
                    {
                        vv.Add(v[j]);
                    }
                }

                for (int j = 0; j < vv.Count; j++)
                {
                    a[Pos(3 - j, c)] = vv[j];
                }

                for (var j = vv.Count; j < 4; j++)
                {
                    a[Pos(3 - j, c)] = null;
                }
            }
        }

        void Rotate()
        {
            //旋转90度
            var c = new Card[16];
            for (var i = 0; i < 16; i++)
            {
                var (x, y) = (i / 4, i % 4);
                c[Pos(x, y)] = a[Pos(y, 3 - x)];
            }

            a = c;
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

        bool HasChange()
        {
            for (var i = 0; i < 16; i++)
            {
                if (snap[i] != a[i])
                {
                    return true;
                }
            }

            return false;
        }

        void Snapshot()
        {
            //执行镜像操作，把数据记录到snap数组里面，用于判断是否发生了合并
            if (snap == null)
            {
                snap = new Card[16];
            }

            for (var i = 0; i < 16; i++) snap[i] = a[i];
        }

        void Render()
        {
            for (var i = 0; i < 16; i++)
            {
                if (a[i] != null)
                {
                    setCardPosition(a[i], i);
                    if (a[i].merged != null)
                    {
                        a[i].merged.moveToAndDestroy(points[i / 4, i % 4]); //移动到某个位置然后销毁
                        a[i].merged = null;
                    }

                    if (a[i].nextValue != a[i].value && a[i].nextValue != 0)
                    {
                        //执行数值变化
                        a[i].mergeGrow();
                        a[i].value = a[i].nextValue;
                        a[i].nextValue = 0;
                        setCardColorAndText(a[i]);
                    }
                }
            }
        }

        void gameOver(bool isWin)
        {
            gameOverPanel.SetActive(true);
            if (isWin)
            {
                gameOverText.text = "Congratulation !";
                AudioWin.Play();
            }
            else
            {
                gameOverText.text = "Game Over!";
                AudioLose.Play();
            }
        }

        void Update()
        {
            var keydown = false;
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                keydown = true;
                Up();
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                keydown = true;
                Down();
            }
            else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                keydown = true;
                Left();
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                keydown = true;
                Right();
            }

            if (keydown)
            {
                if (HasChange())
                {
                    Render();
                    Generate();
                    Snapshot();
                    UpdateGameScore();
                }

                if (checkWin())
                {
                    gameOver(true);
                    return;
                }

                if (checkLose())
                {
                    gameOver(false);
                    return;
                }
            }
        }
    }
}