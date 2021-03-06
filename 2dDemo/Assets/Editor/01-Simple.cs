using NUnit.Framework;
using UnityEngine;

namespace Editor
{
    [TestFixture]
    public class Simple
    {
        [Test]
        public void usePrint()
        {
            //不要使用Debug.Log了，直接使用print
            MonoBehaviour.print("hello world");
        }

        [Test]
        public void twoDimension()
        {
            var a = new Vector2Int[3, 3];
            MonoBehaviour.print(a.Length); //9，说明是一个一维数组的长度
        }
    }
}