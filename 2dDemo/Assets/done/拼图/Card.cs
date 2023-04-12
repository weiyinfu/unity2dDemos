using System.Collections;
using UnityEngine;
using Util;

namespace 拼图
{
    public class Card : MonoBehaviour
    {
        public int value;
        public Pintu pintu;

        private void Start()
        {
        }

        public void setValue(int x)
        {
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.color = Color.blue;
            this.value = x;
            if (x == 0)
            {
                textMesh.text = "";
            }
            else
            {
                textMesh.text = x + "";
            }
        }

        public void moveTo(Vector3 pos)
        {
            var c = StartCoroutine("moveCoroutine", pos);
        }

        private IEnumerator moveCoroutine(Vector3 p)
        {
            var x = AnimateUtil.moveTo(gameObject, p, 0.5f, 10);
            while (x.MoveNext())
            {
                yield return null;
            }

            pintu.playSound(pintu.soundMove);
        }
    }
}