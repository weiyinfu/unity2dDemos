using UnityEngine;
using System.Collections;
/*
 * 方块的动画：有两种动画，AnimateEntry+AnimateUpgrade，分别表示出现+升级
 *
 */
public class TileAnimationHandler : MonoBehaviour
{
    public float scaleSpeed;
    public float growSize;//变大多少
    private Transform _transform;//实际的transform
    private Vector3 growVector;

    public void AnimateEntry()
    {
        StartCoroutine("AnimationEntry");
    }

    public void AnimateUpgrade()
    {
        StartCoroutine("AnimationUpgrade");
    }

    private IEnumerator AnimationEntry()
    {
        while (_transform == null)
        {
            yield return null;
        }

        _transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        while (_transform.localScale.x < 1f)
        {
            _transform.localScale = Vector3.MoveTowards(
                _transform.localScale,
                Vector3.one,
                scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator AnimationUpgrade()
    {
        while (_transform == null)
        {
            yield return null;
        }

        while (_transform.localScale.x < 1f + growSize)
        {
            _transform.localScale = Vector3.MoveTowards(
                _transform.localScale,
                Vector3.one + growVector,
                scaleSpeed * Time.deltaTime
                );
            yield return null;
        }

        while (_transform.localScale.x > 1f)
        {
            _transform.localScale = Vector3.MoveTowards(
                _transform.localScale,
                Vector3.one,
                scaleSpeed * Time.deltaTime
                );
            yield return null;
        }
    }

    void Start()
    {
        _transform = transform;
        growVector = new Vector3(growSize, growSize, 0f);
    }
}