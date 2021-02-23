using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOtherPlayer : UIPlayerBase
{
    protected override void Awake()
    {
        // === auto generated code begin === 
        usernameText = transform.Find("InfoPanel/UsernamePanel/UsernameText").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("InfoPanel/CoinPanel/CoinText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        cardsTransform = transform.Find("Cards");
        headIconImage = transform.Find("InfoPanel/HeadIcon/HeadIconMask/HeadIconImage").GetComponent<Image>();
        timer = transform.Find("Timer").GetComponent<UITimer>();
        passTurnText = transform.Find("PassTurnText").GetComponent<Text>();
    }

}
