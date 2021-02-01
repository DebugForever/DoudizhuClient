using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HintText : MonoBehaviour
{
    private Text text;
    private Color origColor;//组件原本的颜色
    private Color endColor;//动画结束的颜色
    public float hintShowTime = 2.0f;
    public float animationTime = 0.5f;

    private Coroutine tweenCoroutine;//用于记录未完成的协程

    private void Awake()
    {
        text = GetComponent<Text>();

        //设置颜色，注意color是值类型
        origColor = text.color;
        origColor.a = 0;
        text.color = origColor;
        endColor = origColor;
        endColor.a = 1;

        EventCenter.AddListener<string>(EventType.UIFlashHint, ShowText);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventType.UIFlashHint, ShowText);
    }

    void ShowText(string str)
    {
        text.text = str;
        if (tweenCoroutine != null)//停止之前未完成的协程
            StopCoroutine(tweenCoroutine);
        text.DOColor(endColor, animationTime).OnComplete(() => tweenCoroutine = StartCoroutine(TweenBack()));
    }

    IEnumerator TweenBack()
    {
        yield return new WaitForSeconds(hintShowTime);
        text.DOColor(origColor, animationTime);
    }

}
