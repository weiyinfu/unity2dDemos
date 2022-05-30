using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public sealed class ColorUtil
    {
        public List<CardColor> colorList; //颜色列表，为不同的数字使用不同的颜色

        public ColorUtil(int n)
        {
            InitColorList(n);
        }

        static float ColorDistance(Color x, Color y)
        {
            var dr = x.r - y.r;
            var dg = x.g - y.g;
            var db = x.b - y.b;
            return Mathf.Sqrt(dr * dr + dg * dg + db * db);
        }

        private void InitColorList(int n)
        {
            //n是颜色的个数
            colorList = new List<CardColor>();
            int per = Mathf.CeilToInt(Mathf.Pow(n, 1 / 3.0f));
            for (int i = 0; i < per; i++)
            {
                for (int j = 0; j < per; j++)
                {
                    for (int k = 0; k < per; k++)
                    {
                        var back = new Color((float) i / per, (float) j / per, (float) k / per);
                        Color fore;
                        //判断前景色使用白色还是黑色，根据哪一种颜色区分度比较大进行判断
                        if (ColorDistance(back, Color.white) < ColorDistance(back, Color.black))
                        {
                            fore = Color.black;
                        }
                        else
                        {
                            fore = Color.white;
                        }

                        colorList.Add(new CardColor(back, fore));
                        if (colorList.Count >= n)
                        {
                            Shuffle(colorList);
                            return;
                        }
                    }
                }
            }
        }

        static void Shuffle(List<CardColor> cardColors)
        {
            for (int i = 0; i < cardColors.Count; i++)
            {
                int ind = Mathf.FloorToInt(Random.value * (cardColors.Count - i) + i);
                (cardColors[ind], cardColors[i]) = (cardColors[i], cardColors[ind]);
            }
        }
    }
}