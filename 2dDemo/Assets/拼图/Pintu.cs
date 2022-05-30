using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace 拼图
{
    public class Pintu : MonoBehaviour
    {
        public GameObject cardPrefab;
        public AudioClip soundWin;
        public AudioClip soundStart;
        public AudioClip soundMove;
        private int rows = 2;
        private int cols = 2;
        private Card[,] a;
        private Vector3[,] positions;
        private GameObject panelOver;
        private GameObject panelConfig;
        private Camera mainCamera;
        private static Dictionary<AudioClip, AudioSource> audioSources = new Dictionary<AudioClip, AudioSource>();

        void Start()
        {
            panelOver = GameObject.Find("PanelOver");
            panelConfig = GameObject.Find("PanelConfig");
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            GameObject.Find("ButtonNewGame").GetComponent<Button>().onClick.AddListener(() => { showConfig(); });
            GameObject.Find("ButtonStart").GetComponent<Button>().onClick.AddListener(() =>
            {
                var rows = Int32.Parse(GameObject.Find("InputFieldRows").GetComponent<TMP_InputField>().text);
                var cols = Int32.Parse(GameObject.Find("InputFieldCols").GetComponent<TMP_InputField>().text);
                panelConfig.SetActive(false);
                this.rows = rows;
                this.cols = cols;
                newGame(rows, cols);
            });
            showConfig();
        }

        //清空旧的GameObject
        void clear()
        {
            var a = FindObjectsOfType<Card>();
            print($"clear is called:{a.Length}");
            foreach (var i in a)
            {
                Destroy(i.gameObject);
            }
        }

        void showConfig()
        {
            panelOver.SetActive(false);
            panelConfig.SetActive(true);
        }

        public void playSound(AudioClip clip)
        {
            if (!audioSources.ContainsKey(clip))
            {
                var obj = new GameObject();
                var audioSource = obj.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSources[clip] = audioSource;
            }

            {
                var audioSource = audioSources[clip];
                audioSource.Play();
            }
        }

        void newGame(int rows, int cols)
        {
            playSound(soundStart);
            clear();
            a = new Card[rows, cols];
            positions = new Vector3[rows, cols];
            var sz = mainCamera.orthographicSize;
            var (width, height) = (sz * mainCamera.aspect * 2, sz * 2);
            var (centerX, centerY) = ((cols - 1) / 2.0f, (rows - 1) / 2.0f);
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    positions[i, j] = new Vector3((j - centerX) * width / cols, (centerY - i) * height / rows, 0);
                }
            }

            var initValue = new int[rows, cols];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    initValue[i, j] = (i * cols + j + 1) % (rows * cols);
                }
            }

            shuffle(initValue);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (initValue[i, j] == 0)
                    {
                        a[i, j] = null;
                        continue;
                    }

                    var obj = Instantiate(cardPrefab);
                    var card = obj.GetComponent<Card>();
                    var objRect = obj.GetComponent<RectTransform>().rect;
                    a[i, j] = card;
                    card.pintu = this;
                    obj.name = $"card{i}-{j}";
                    card.setValue(initValue[i, j]);
                    obj.transform.localScale = new Vector3(
                        width / cols * 0.9f / objRect.width,
                        height / rows * 0.9f / objRect.height);
                    obj.transform.SetPositionAndRotation(positions[i, j], Quaternion.identity);
                }
            }
        }

        void shuffle(int[,] a)
        {
            var directions = new[] {(0, 1), (0, -1), (1, 0), (-1, 0)};
            var space = new Vector2Int(rows - 1, cols - 1);
            for (var i = 0; i < a.Length * 40; i++)
            {
                List<Vector2Int> candidates = new List<Vector2Int>();
                foreach (var (dx, dy) in directions)
                {
                    var now = new Vector2Int(space.x + dx, space.y + dy);
                    if (!legal(now.x, now.y))
                    {
                        continue;
                    }

                    candidates.Add(now);
                }

                var des = candidates[Random.Range(0, candidates.Count)];
                (a[space.x, space.y], a[des.x, des.y]) = (a[des.x, des.y], a[space.x, space.y]);
                space = des;
            }
        }

        bool legal(int x, int y)
        {
            return x >= 0 && y >= 0 && x < rows && y < cols;
        }

        bool isOver()
        {
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (a[i, j] == null) continue;
                    if (a[i, j].value != i * cols + j + 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void moveCard(int fx, int fy, int tx, int ty)
        {
            //把一个物体移动到des，在0.5s的时间内
            if (!legal(fx, fy))
            {
                return;
            }

            var obj = a[fx, fy];
            if (obj == null)
            {
                return;
            }

            obj.moveTo(positions[tx, ty]);
            (a[fx, fy], a[tx, ty]) = (a[tx, ty], a[fx, fy]);
            if (isOver())
            {
                playSound(soundWin);
                panelOver.SetActive(true);
            }
        }

        void Update()
        {
            if (a == null) return;
            var spaceX = -1;
            var spaceY = -1;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var obj = a[i, j];
                    if (obj == null)
                    {
                        spaceX = i;
                        spaceY = j;
                        goto over;
                    }
                }
            }

            over: ;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveCard(spaceX - 1, spaceY, spaceX, spaceY);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveCard(spaceX, spaceY + 1, spaceX, spaceY);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveCard(spaceX, spaceY - 1, spaceX, spaceY);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveCard(spaceX + 1, spaceY, spaceX, spaceY);
            }
        }
    }
}