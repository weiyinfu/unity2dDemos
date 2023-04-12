using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Util;

namespace GreedySnake
{
    public class SnakeBody : MonoBehaviour
    {
        public int ind;
        public Vector2Int direction;
        public Snake snake;

        public void moveTo(Vector3 point)
        {
            StartCoroutine("moveToCoroutine", point);
        }

        private IEnumerator moveToCoroutine(Vector3 p)
        {
            var a = AnimateUtil.moveTo(gameObject, p, snake.moveTimeDelta, snake.moveFrameCount);
            while (a.MoveNext()) yield return null;
        }
    }

    class Food : MonoBehaviour
    {
        public int ind;
    }

    enum GameState
    {
        over,
        running,
    }

    public class Snake : MonoBehaviour
    {
        public GameObject bodyPrefab;

        public GameObject foodPrefab;

        public GameObject wallPrefab;

        public AudioSource audioSourceEat;

        public AudioSource audioSourceCollide;

        public AudioSource audioSourceStart;
        public AudioSource[] audioSourceList;
        public float moveTimeDelta = 3f;
        public int moveFrameCount = 5;

        private float lastMoveTime = 0;

        // Start is called before the first frame update
        private Camera mainCamera;
        private int rows = 20, cols = 40; //行数和列数
        private List<SnakeBody> snake;

        private Vector2Int snakeDirection = Vector2Int.left;

        /*
         * 如果直接用snakeDirection存储方向，会出现蛇头逆向的问题，而实际上蛇头是不允许逆向的
         * 例如正在往上走，这时候按下是不允许的；但是按下左右是允许的。先按左右，然后数据存储到了snakeDirection，紧接着再按下。这样就会出现蛇头撞自己的情况
         */
        private Vector2Int inputDirection = Vector2Int.left;
        private Vector3[] points;
        private Food food;
        private GameState gameState;
        private float Width, Height;
        private GameObject gameOverPanel;
        private ColorUtil colorUtil;

        void Start()
        {
            gameOverPanel = GameObject.Find("PanelGameOver");
            gameOverPanel.GetComponent<Button>().onClick.AddListener(newGame);
            gameOverPanel.SetActive(false);
            mainCamera = FindObjectOfType<Camera>();
            var (pointList, width, height) = Util.GridManager.getGrid(mainCamera, rows, cols);
            points = new Vector3[pointList.Length];
            Width = width;
            Height = height;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    points[i * cols + j] = pointList[i, j];
                }
            }

            colorUtil = new ColorUtil(rows * cols);
            buildWall();
            newGame();
        }

        void clearSnakeAndFood()
        {
            //清除掉过去的蛇和食物，准备重新创建游戏
            var food = FindObjectsOfType<Food>();
            var snake = FindObjectsOfType<SnakeBody>();
            foreach (var i in food)
            {
                Destroy(i.gameObject);
            }

            foreach (var i in snake)
            {
                Destroy(i.gameObject);
            }
        }

        void newGame()
        {
            audioSourceStart.Play();
            gameOverPanel.SetActive(false);
            clearSnakeAndFood();
            snake = new List<SnakeBody>();
            snake.Add(newBody(rows / 2 * cols + cols / 2));
            addNewFood();
            snakeDirection = Vector2Int.up;
            lastMoveTime = 0;
            gameState = GameState.running;
        }

        void addNewFood()
        {
            var snakeSet = new Collection<int>();
            foreach (var i in snake)
            {
                snakeSet.Add(i.ind);
            }

            var candidates = new List<int>();
            for (var i = 1; i < rows - 1; i++)
            {
                for (var j = 1; j < cols - 1; j++)
                {
                    var ind = i * cols + j;
                    if (snakeSet.Contains(ind))
                    {
                        continue;
                    }

                    candidates.Add(ind);
                }
            }

            if (candidates.Count == 0)
            {
                gameOver();
                return;
            }

            var pos = candidates[Random.Range(0, candidates.Count)];
            this.food = newFood(pos);
        }

        int randomPos()
        {
            var x = Random.Range(1, rows - 1);
            var y = Random.Range(1, cols - 1);
            return x * cols + y;
        }

        void buildWall()
        {
            for (var i = 0; i < rows; i++)
            {
                newWall(i * cols);
                newWall(i * cols + cols - 1);
            }

            for (var i = 1; i < cols - 1; i++)
            {
                newWall(i);
                newWall((rows - 1) * cols + i);
            }
        }

        GameObject newWall(int ind)
        {
            var g = Instantiate(wallPrefab);
            var rec = g.GetComponent<RectTransform>().rect;
            g.transform.position = points[ind];
            g.transform.localScale = new Vector3(Width / cols / rec.width, Height / rows / rec.height);
            return g;
        }

        SnakeBody newBody(int ind)
        {
            var g = Instantiate(bodyPrefab);
            var rect = g.GetComponent<RectTransform>().rect;
            var body = g.AddComponent<SnakeBody>();
            var renderer = g.GetComponent<SpriteRenderer>();
            body.ind = ind;
            body.snake = this;
            renderer.color = colorUtil.colorList[Random.Range(0, colorUtil.colorList.Count)].back;
            g.transform.position = points[ind];
            g.transform.localScale = new Vector3(Width / cols / rect.width, Height / rows / rect.height);
            return body;
        }

        Food newFood(int ind)
        {
            var g = Instantiate(foodPrefab);
            var f = g.AddComponent<Food>();
            var rect = g.GetComponent<RectTransform>().rect;
            f.ind = ind;
            g.transform.position = points[ind];
            g.transform.localScale = new Vector3(Width / cols / rect.width, Height / rows / rect.height);
            return f;
        }

        bool legal(int x, int y)
        {
            return x >= 1 && y >= 1 && x < rows - 1 && y < cols - 1;
        }

        void gameOver()
        {
            gameState = GameState.over;
            audioSourceCollide.Play();
            gameOverPanel.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (gameState == GameState.over)
            {
                return;
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                inputDirection = Vector2Int.right;
            }
            else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                inputDirection = Vector2Int.down;
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                inputDirection = Vector2Int.up;
            }
            else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                inputDirection = Vector2Int.left;
            }

            if (lastMoveTime + moveTimeDelta > Time.realtimeSinceStartup)
            {
                //如果距离上次移动时间小于moveTimeDelta
                return;
            }

            if (inputDirection == -snakeDirection || inputDirection == snakeDirection)
            {
                //如果是反方向，则是不允许的操作
            }
            else
            {
                snakeDirection = inputDirection;
            }


            lastMoveTime = Time.realtimeSinceStartup;

            var head = snake[0];
            var headPos = new Vector2Int(head.ind / cols, head.ind % cols);
            var nex = headPos + snakeDirection;
            if (!legal(nex.x, nex.y))
            {
                print($"撞墙了nextPosition={nex} headPosition={headPos} snakeDirection={snakeDirection}");
                gameOver();
                return;
            }

            var nexInd = nex.x * cols + nex.y;
            //检查是否撞到了自己
            for (int i = 0; i < snake.Count; i++)
            {
                if (snake[i].ind == nexInd)
                {
                    gameOver();
                    return;
                }
            }

            bool grow = nexInd == food.ind;
            var last = nexInd;
            for (int i = 0; i < snake.Count; i++)
            {
                snake[i].moveTo(points[last]);
                (snake[i].ind, last) = (last, snake[i].ind);
            }

            if (grow)
            {
                //蛇的身体增长一节
                snake.Add(newBody(last));
                //生成新食物
                DestroyImmediate(this.food.gameObject);
                addNewFood(); //生成一个新的食物
                audioSourceEat.Play();
            }
        }
    }
}