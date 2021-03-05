using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class GameManagerOnline : MonoBehaviour
{
    private GameView view;
    private MainPlayerManager p1Manager;
    private OtherPlayerManager p2Manager;
    private OtherPlayerManager p3Manager;

    void Awake()
    {
        view = GetComponentInChildren<GameView>();
        p1Manager = transform.Find("MainPlayer").GetComponent<MainPlayerManager>();
        p1Manager.Init(CardManager.MainPlayerHand);
        p2Manager = transform.Find("Player2").GetComponent<OtherPlayerManager>();
        p2Manager.Init(CardManager.Player2Hand, 2);
        p3Manager = transform.Find("Player3").GetComponent<OtherPlayerManager>();
        p3Manager.Init(CardManager.Player3Hand, 3);

        EventCenter.AddListener(EventType.TestEvent, Test);

        CardManager.SetLastHand(CardSet.None);

    }

    private void Start()
    {
        view.player2View.HideInfo();
        view.player3View.HideInfo();
        view.under3Cards.HideNoAnim();

        //进入游戏之后直接匹配，不整什么按钮了
        NetMsgCenter.Instance.SendEnterRoomMsg();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.TestEvent, Test);

    }

    private void Test()
    {

    }

}
