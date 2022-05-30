using UnityEngine;

namespace 万向贪吃蛇
{
    public class Snake : MonoBehaviour
    {
        public float snakeSpeed = 0.5f;
        public GameObject foodPrefab;
        public GameObject bodyPrefab;
        public GameObject wallPrefab;
        private Camera mainCamera;
        private RectTransform gameContent;
        private Vector3[] points;
        private int Rows = 10;
        private int Cols = 20;
        private float Width;
        private float Height;

        private void Start()
        {
            mainCamera = FindObjectOfType<Camera>();

            gameContent = GameObject.Find("GameContent").GetComponent<RectTransform>();
            var Rec = gameContent.rect;
            Rec.width = 30 * Cols;
            Rec.height = 30 * Rows;
            gameContent.sizeDelta = new Vector2(Rec.width, Rec.height);
            Width = Rec.width;
            Height = Rec.height;
            mainCamera.orthographicSize = Mathf.Max(Rec.width / mainCamera.aspect / 2, Rec.height / 2);
            var points = Util.GridManager.getGridOfRectTransform(gameContent, Rows, Cols);
            this.points = new Vector3[Rows * Cols];
            for (var i = 0; i < Rows * Cols; i++)
            {
                this.points[i] = points[i / Cols, i % Cols];
            }

            buildWall();
            initSnakeHead();
        }

        GameObject newWall(int ind)
        {
            var g = Instantiate(wallPrefab, gameContent.transform, false);
            var rec = g.GetComponent<RectTransform>().rect;
            g.transform.position = points[ind];
            g.transform.localScale = new Vector3(Width / Cols / rec.width, Height / Rows / rec.height);
            return g;
        }

        void buildWall()
        {
            for (var i = 0; i < Rows; i++)
            {
                newWall(i * Cols);
                newWall(i * Cols + Cols - 1);
            }

            for (var i = 1; i < Cols - 1; i++)
            {
                newWall(i);
                newWall((Rows - 1) * Cols + i);
            }
        }

        void initSnakeHead()
        {
            var head = Instantiate(bodyPrefab, gameContent.transform, false);
            var rec = head.GetComponent<RectTransform>().rect;
            head.transform.localPosition = randomPosition();
            head.transform.localScale = new Vector3(Width / Cols / rec.width, Height / Rows / rec.height);
            head.name = "SnakeHead";
        }

        Vector3 randomPosition()
        {
            var cnt = 2;
            return new Vector3(
                Random.Range(Width / Cols * cnt, gameContent.rect.width - Width / Cols * cnt),
                Random.Range(Height / Rows * cnt, gameContent.rect.height - Height / Rows * cnt)
            );
        }

        void generateFood()
        {
            //生成食物
            var foo = Instantiate(foodPrefab, gameContent.transform, false);
            var rec = foo.GetComponent<RectTransform>().rect;
            foo.transform.localPosition = randomPosition();
            foo.transform.localScale = new Vector3(Width / Cols / rec.width, Height / Rows / rec.height);
        }

        private int lastTime;

        void Update()
        {
            var now = (int) Time.realtimeSinceStartup;
            if (now % 30 == 0 && now != lastTime)
            {
                print($"now={now} lastTime={lastTime} generating food");
                lastTime = now;
                generateFood();
            }

            var snakeHead = GameObject.Find("SnakeHead");
            var p = Input.mousePosition;
            p = mainCamera.ScreenToWorldPoint(p);
            var head = snakeHead.transform.position;
            p.z = head.z;
            var ratio = snakeSpeed * Time.deltaTime / Vector3.Distance(head, p);
            ratio = Mathf.Min(ratio, 1);
            var nextPosition = Vector3.Lerp(head, p, ratio);
            snakeHead.transform.position = nextPosition;
        }
    }
}