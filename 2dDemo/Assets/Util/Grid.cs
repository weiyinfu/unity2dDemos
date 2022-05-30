using UnityEngine;

namespace Util
{
    public class GridManager
    {
        public static (Vector3[,] positions, float width, float height) getGrid(Camera mainCamera, int rows, int cols)
        {
            var positions = new Vector3[rows, cols];
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

            return (positions, width, height);
        }

        public static Vector3[,] getGridOfRectTransform(RectTransform rect, int rows, int cols)
        {
            var positions = new Vector3[rows, cols];
            var (centerX, centerY) = ((cols - 1) / 2.0f, (rows - 1) / 2.0f);
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    positions[i, j] = new Vector3((j - centerX) * rect.rect.width / cols, (centerY - i) * rect.rect.height / rows, 0);
                }
            }

            return positions;
        }
    }
}