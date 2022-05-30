using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Util
{
    public class AnimateUtil
    {

        //在duration时间内移动到p位置
        public static IEnumerator moveTo(GameObject gameObject, Vector3 p, float duration, int frameCount)
        {
            var o = gameObject.transform.position;
            int total = frameCount;
            var startTime = Time.realtimeSinceStartup;
            for (var i = 1; i < total; i++)
            {
                while (true)
                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio > 1.0f * i / total) break;
                    yield return null;
                }

                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio < 0) ratio = 0f;
                    if (ratio > 1) break;
                    gameObject.transform.position = p * ratio + o * (1 - ratio);
                    yield return null;
                }
            }

            gameObject.transform.position = p;
        }


        //在duration时间内移动到p位置
        public static IEnumerator moveToByLocalPosition(GameObject gameObject, Vector3 p, float duration, int frameCount)
        {
            var o = gameObject.transform.localPosition;
            int total = frameCount;
            var startTime = Time.realtimeSinceStartup;
            for (var i = 1; i < total; i++)
            {
                while (true)
                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio > 1.0f * i / total) break;
                    yield return null;
                }

                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio < 0) ratio = 0f;
                    if (ratio > 1) break;
                    gameObject.transform.localPosition = p * ratio + o * (1 - ratio);
                    yield return null;
                }
            }

            gameObject.transform.localPosition = p;
        }


        //在duration时间内缩放到某个向量
        public static IEnumerator scaleTo(GameObject gameObject, Vector3 p, float duration, int frameCount)
        {
            var o = gameObject.transform.localScale;
            int total = frameCount;
            var startTime = Time.realtimeSinceStartup;
            for (var i = 1; i < total; i++)
            {
                while (true)
                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio > 1.0f * i / total) break;
                    yield return null;
                }

                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio < 0) ratio = 0f;
                    if (ratio > 1) break;
                    gameObject.transform.localScale = p * ratio + o * (1 - ratio);
                    yield return null;
                }
            }

            gameObject.transform.localScale = p;
        }

        public static IEnumerator growAndShrink(GameObject gameObject, Vector3 middleScale, Vector3 finalScale, float duration, int frameCount)
        {
            var o = gameObject.transform.localScale;
            int total = frameCount;
            var A = o; //x=0
            var B = middleScale; //x=0.5
            var C = finalScale; //x=1
            //使用二次函数
            var c = A;
            var a = 2 * (C - A - 2 * (B - C));
            var b = C - A - a;
            //使用二次函数表示大小
            var startTime = Time.realtimeSinceStartup;
            for (var i = 1; i < total; i++)
            {
                while (true)
                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio > 1.0f * i / total) break;
                    yield return null;
                }

                {
                    var ratio = (Time.realtimeSinceStartup - startTime) / duration;
                    if (ratio < 0) ratio = 0f;
                    if (ratio > 1) break;
                    var x = ratio;
                    var y = a * x * x + b * x + c;
                    gameObject.transform.localScale = y;
                    yield return null;
                }
            }

            gameObject.transform.localScale = finalScale;
        }
    }
}