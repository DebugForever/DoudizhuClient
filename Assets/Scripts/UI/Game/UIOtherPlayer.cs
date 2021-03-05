using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOtherPlayer : UIPlayerBase
{
    private GameObject go_InfoPanel;
    protected override void Awake()
    {
        // === auto generated code begin === 
        usernameText = transform.Find("InfoPanel/UsernamePanel/UsernameText").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("InfoPanel/CoinPanel/CoinText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        cardsTransform = transform.Find("Cards");
        headIconImage = transform.Find("InfoPanel/HeadIcon/HeadIconMask/HeadIconImage").GetComponent<Image>();
        timer = transform.Find("Timer").GetComponent<UITimer>();
        statusText = transform.Find("StatusText").GetComponent<Text>();
        lastHandCards = transform.Find("LastHandCards").GetComponent<UILastHandCards>();

        go_InfoPanel = transform.Find("InfoPanel").gameObject;
    }

    /// <summary>
    /// 隐藏用户信息面板
    /// </summary>
    public void HideInfo()
    {
        go_InfoPanel.SetActive(false);
    }

    /// <summary>
    /// 显示用户信息面板
    /// </summary>
    public void ShowInfo()
    {
        go_InfoPanel.SetActive(true);
    }
}
