using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace 扫雷
{
    class Card : MonoBehaviour
    {
        public int value;
        public bool isDanger;
        public bool handled;

        public void setText(string s)
        {
            var text = GetComponentInChildren<TextMeshPro>();
            text.text = s;
        }
    }

    enum GameState
    {
        unsure,
        win,
        lose
    }

    public class Saolei : MonoBehaviour
    {
        public AudioSource audioBomb;
        public AudioSource audioLose;
        public AudioSource audioWin; //胜利的时候播放的声音
        public AudioSource audioStart;
        public AudioSource audioClick;
        public GameObject prefabCard;

        private RectTransform gameContent;
        private Vector3[,] points;
        private Card[,] a;
        private Camera mainCamera;
        private GameState state = GameState.win;

        private GameObject configPanel;
        GameObject gameOverPanel;
        int Rows = 5;
        int Cols = 10;
        float danderRatio = 0.3f;

        void Start()
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
            gameOverPanel.SetActive(false);

            gameOverPanel.GetComponent<Button>().onClick.AddListener(showConfigPanel);
            mainCamera = FindObjectOfType<Camera>();
            gameContent = GameObject.Find("GameContent").GetComponent<RectTransform>();

            configPanel = GameObject.Find("GameConfigPanel");
            GameObject.Find("GameConfigPanel/Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                var sliderRow = GameObject.Find("Rows/Slider").GetComponent<Slider>();
                var sliderCol = GameObject.Find("Cols/Slider").GetComponent<Slider>();
                var sliderDifficulty = GameObject.Find("Difficulty/Slider").GetComponent<Slider>();
                this.Rows = (int) sliderRow.value;
                this.Cols = (int) sliderCol.value;
                this.danderRatio = sliderDifficulty.value;
                newGame();
            });

            showConfigPanel();
        }

        void showConfigPanel()
        {
            gameOverPanel.SetActive(false);
            configPanel.SetActive(true);
        }

        void newGame()
        {
            state = GameState.unsure;
            configPanel.SetActive(false);
            foreach (var i in FindObjectsOfType<Card>())
            {
                Destroy(i.gameObject);
            }

            a = new Card[Rows, Cols];
            gameContent.sizeDelta = new Vector2(30 * Cols, 30 * Rows);
            var Rec = gameContent.rect;
            Rec.width = 30 * Cols;
            Rec.height = 30 * Rows;
            mainCamera.orthographicSize = Mathf.Max(Rec.width / mainCamera.aspect / 2, Rec.height / 2);
            points = Util.GridManager.getGridOfRectTransform(gameContent, Rows, Cols);
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    var obj = Instantiate(prefabCard);
                    var c = obj.AddComponent<Card>();
                    c.transform.SetParent(gameContent.transform);
                    c.name = $"Card{i}-{j}";
                    var rect = c.gameObject.GetComponent<RectTransform>().rect;
                    c.transform.localPosition = points[i, j];
                    c.isDanger = false;
                    c.transform.localScale = new Vector3(0.95f, 0.95f);
                    a[i, j] = c;
                }
            }

            //随机布雷，设置value
            var badCount = Mathf.Round(Rows * Cols * danderRatio);
            print($"bad count{badCount}");
            for (var i = 0; i < badCount; i++)
            {
                var x = Random.Range(0, Rows);
                var y = Random.Range(0, Cols);
                a[x, y].isDanger = true;
            }

            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    var danger = 0;
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        for (var dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0)
                            {
                                continue;
                            }

                            var (neiborX, neiborY) = (i + dx, j + dy);
                            if (!legal(neiborX, neiborY)) continue;
                            if (a[neiborX, neiborY].isDanger)
                            {
                                danger++;
                            }
                        }
                    }

                    a[i, j].value = danger;
                }
            }

            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    a[i, j].setText("");
                }
            }

            audioStart.Play();
        }

        void showAll()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    if (a[i, j].isDanger)
                    {
                        a[i, j].setText("雷");
                    }
                    else
                    {
                        a[i, j].setText(a[i, j].value + "");
                    }
                }
            }
        }

        bool legal(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Rows && y < Cols;
        }

        Vector2Int getPosition()
        {
            var p = Mouse.current.position.value;
            p = mainCamera.ScreenToWorldPoint(p);
            var rect = gameContent.rect;
            var (w, h) = (rect.width / Cols, rect.height / Rows);
            var (rx, ry) = (p.x - rect.x, p.y - rect.y);
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    var t = a[i, j].transform.position;
                    if (Mathf.Abs(p.x - t.x) < w / 2 && Mathf.Abs(p.y - t.y) < h / 2)
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }

            return Vector2Int.one * -1;
        }

        void gameOver(bool isWin)
        {
            print($"gameOver={isWin}");
            var txt = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (isWin)
            {
                txt.text = "Congratulations!";
                audioWin.Play();
                state = GameState.win;
            }
            else
            {
                txt.text = "Game Over!";
                showAll();
                audioLose.Play();
                state = GameState.lose;
            }

            gameOverPanel.SetActive(true);
        }

        void handle(int mouseButton)
        {
            var p = getPosition();
            if (p.x == -1)
            {
                return;
            }

            var card = a[p.x, p.y];
            audioClick.Play();
            if (card.isDanger)
            {
                card.setText("雷");
                if (mouseButton == 0)
                {
                    gameOver(false);
                }
                else
                {
                    card.handled = true;
                }
            }
            else
            {
                if (mouseButton == 0)
                {
                    card.setText(card.value + "");
                    card.handled = true;
                }
                else
                {
                    card.setText("雷");
                }
            }

            if (checkWin())
            {
                gameOver(true);
            }
        }

        bool checkWin()
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    if (!a[i, j].handled)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Update()
        {
            if (state == GameState.unsure)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    handle(0);
                }
                else if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    handle(1);
                }
            }
        }
    }
}