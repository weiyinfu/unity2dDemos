using UnityEngine;

namespace Util
{
    public class CardColor
    {
        public Color back; //卡片的背景色
        public Color fore; //卡片的前景色

        public CardColor(Color back, Color fore)
        {
            this.back = back;
            this.fore = fore;
        }
    }
}