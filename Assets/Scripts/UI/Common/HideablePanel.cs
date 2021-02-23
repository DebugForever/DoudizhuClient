using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HideablePanel : MonoBehaviour
{
    /// <summary>
    /// 显示该UI
    /// </summary>
    public virtual void Show()
    {
        ShowNoAnim();
    }

    /// <summary>
    /// 隐藏该UI
    /// </summary>
    public virtual void Hide()
    {
        HideNoAnim();
    }

    /// <summary>
    /// 立即隐藏该UI，无动画
    /// </summary>
    public virtual void HideNoAnim()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 立即显示该UI，无动画
    /// </summary>
    public virtual void ShowNoAnim()
    {
        gameObject.SetActive(true);
    }
}
