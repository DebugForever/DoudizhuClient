﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class GameManagerOffline : MonoBehaviour
{
    private GameView view;
    private MainPlayerManager p1Manager;
    private OtherPlayerManager p2Manager;
    private OtherPlayerManager p3Manager;

    //回合管理，之后可能会独立出来
    private int currentPlayer = 1;
    private bool turnStarted = false;
    private bool turnEnded = true;

    /// <summary>连续不出的人数，为2则代表没人压牌，可以自己任意出</summary>
    private int passCount = 0;

    /// <summary>
    /// 结束回合
    /// </summary>
    public void EndTurn()
    {
        if (turnStarted)
        {
            turnEnded = true;
            turnStarted = false;
            passCount = 0;
        }
    }

    /// <summary>
    /// 不出牌结束回合
    /// </summary>
    public void EndTurnPass()
    {
        if (turnStarted)
        {
            turnEnded = true;
            turnStarted = false;
            passCount += 1;
            if (passCount >= 2)//没人压牌，直接替换为None牌型
            {
                CardManager.SetLastHand(CardSet.None);
            }
        }
    }


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
        EventCenter.AddListener<CardSet>(EventType.PlayerPlayCard, PlayerPlayCard);
        EventCenter.AddListener(EventType.PlayCardHint, PlayCardHint);

        CardManager.SetLastHand(CardSet.None);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.TestEvent, Test);
        EventCenter.RemoveListener<CardSet>(EventType.PlayerPlayCard, PlayerPlayCard);
        EventCenter.RemoveListener(EventType.PlayCardHint, PlayCardHint);
    }

    private void FixedUpdate()
    {
        if (!turnStarted && turnEnded)
        {
            StartTurn();
            currentPlayer += 1;
            if (currentPlayer > 3)
                currentPlayer = 1;
        }
    }

    /// <summary>
    /// 一名玩家的回合
    /// </summary>
    private void StartTurn()
    {
        //回合开始，开协程计时60
        //等待出牌或者计时结束
        //出牌 - 终止协程
        //计时结束 - 自动不出，先手则自动选择出牌
        //下一回合
        if (currentPlayer == 1)
        {
            p1Manager.StartTurn();
            turnStarted = true;
        }
        else if (currentPlayer == 2)
        {
            p2Manager.StartTurn();
            turnStarted = true;
        }
        else if (currentPlayer == 3)
        {
            p3Manager.StartTurn();
            turnStarted = true;
        }
    }

    private void Test()
    {
        StartCoroutine(CardManager.DealCardCoroutine());
    }



    private void PlayCardHint()
    {
        EventCenter.BroadCast(EventType.UIFlashHint, "提示功能还没做。。。");
    }

    private void PlayerPlayCard(CardSet cardSet)
    {
        view.lastHandCards.ClearCards();
        view.lastHandCards.SetCards(cardSet.Cards);
    }
}
