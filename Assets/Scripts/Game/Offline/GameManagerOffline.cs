using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码
using ServerProtocol.SharedCode;
public class GameManagerOffline : MonoBehaviour
{
    private GameView view;
    private MainPlayerManagerOffline p1Manager;
    private OtherPlayerManagerOffline p2Manager;
    private OtherPlayerManagerOffline p3Manager;

    //回合管理，之后可能会独立出来
    private int currentPlayer = 1;
    private bool turnStarted = false;
    private bool turnEnded = true;
    private bool turnEnabled = false;
    private int noGrabLandlordPlayerCount = 0;
    private int grabLandlordPlayerCount = 0;
    private int lastGrabLandlordPlayer = 0;
    /// <summary>第一个开始回合（抢地主）的是谁</summary>
    private int startPlayer = 1;
    /// <summary>上局游戏谁赢了</summary>
    private int lastWinner = 1;
    private int landlordPlayer = 0;

    /// <summary>连续不出的人数，为2则代表没人压牌，可以自己任意出</summary>
    private int passCount = 0;


    /// <summary>
    /// 一名玩家的回合
    /// </summary>
    private void StartTurn(TurnType turnType)
    {
        if (currentPlayer == 1)
        {
            turnStarted = true;
            p1Manager.StartTurn(turnType);
        }
        else if (currentPlayer == 2)
        {
            turnStarted = true;
            p2Manager.StartTurn(turnType);
        }
        else if (currentPlayer == 3)
        {
            turnStarted = true;
            p3Manager.StartTurn(turnType);
        }
    }

    /// <summary>
    /// 出牌结束回合，其他类使用
    /// </summary>
    public void EndTurnPlayCard()
    {
        if (turnStarted)
        {
            EndTurnCommon();
            passCount = 0;
            AudioManager.Instance.PlaySendCard();
        }
    }

    /// <summary>
    /// 不出牌结束回合，其他类使用
    /// </summary>
    public void EndTurnPass()
    {
        if (turnStarted)
        {
            EndTurnCommon();
            passCount += 1;
            if (passCount >= 2)//没人压牌，直接替换为None牌型
            {
                CardManager.SetLastHand(CardSet.None);
            }
        }
    }

    //improve 重构抢地主，这里写的很丑
    public void EndTurnGrabLandlord(bool isGrab)
    {
        if (turnStarted)
        {
            if (isGrab)
            {
                lastGrabLandlordPlayer = currentPlayer;
                grabLandlordPlayerCount += 1;
            }
            else
            {
                noGrabLandlordPlayerCount += 1;
            }

            //两个人不抢，抢地主的人当地主
            if (noGrabLandlordPlayerCount >= 2 && lastGrabLandlordPlayer != 0)
            {
                GetPlayerManager(lastGrabLandlordPlayer).BecomeLandlord();
                landlordPlayer = lastGrabLandlordPlayer;
                GrabLandlordOver();
                return;
            }

            //大于等于两个人抢地主，先手玩家有一次额外抢地主的机会
            if (grabLandlordPlayerCount >= 2 && currentPlayer == startPlayer)
            {
                if (isGrab)
                {
                    GetCurrentPlayerManager().BecomeLandlord();
                    landlordPlayer = currentPlayer;
                }
                else
                {
                    GetPlayerManager(lastGrabLandlordPlayer).BecomeLandlord();
                    landlordPlayer = lastGrabLandlordPlayer;
                }
                GrabLandlordOver();
                return;
            }

            //三个人不抢，直接重开
            if (noGrabLandlordPlayerCount >= 3 && grabLandlordPlayerCount == 0)
            {
                MatchReset();
                return;
            }

            EndTurnCommon();
        }
    }

    /// <summary>
    /// 结束抢地主阶段
    /// </summary>
    private void GrabLandlordOver()
    {
        view.under3Cards.CardsFaceUp();
        currentPlayer = landlordPlayer;
        CardManager.GiveUnderCards(landlordPlayer);
        turnEnded = true;
        turnStarted = false;
    }

    /// <summary>
    /// 结束回合共同的代码
    /// </summary>
    private void EndTurnCommon()
    {
        turnEnded = true;
        turnStarted = false;
        if (GetCurrentPlayerManager().GetRemainCardCount() == 0)
        {
            MatchOver();
        }

        currentPlayer += 1;
        if (currentPlayer > 3)
            currentPlayer = 1;
    }

    private void MatchOver()
    {
        GamePause();//游戏结束时需要暂停现有的计时等游戏功能
        lastWinner = currentPlayer;
        EventCenter.BroadCast(EventType.MatchOver);
    }

    private void GamePause()
    {
        turnEnabled = false;
    }

    private PlayerManagerOfflineBase GetCurrentPlayerManager()
    {
        return GetPlayerManager(currentPlayer);
    }

    private PlayerManagerOfflineBase GetPlayerManager(int playerIndex)
    {
        switch (playerIndex) // todo 这里的index与联机版不一样，应该统一从0开始。 
        {
            case 1:
                return p1Manager;
            case 2:
                return p2Manager;
            case 3:
                return p3Manager;
            default:
                return null;
        }
    }

    void Awake()
    {
        view = GetComponentInChildren<GameView>();
        p1Manager = transform.Find("MainPlayer").GetComponent<MainPlayerManagerOffline>();
        p1Manager.Init(CardManager.MainPlayerHand);
        p2Manager = transform.Find("Player2").GetComponent<OtherPlayerManagerOffline>();
        p2Manager.Init(CardManager.Player2Hand, 2);
        p3Manager = transform.Find("Player3").GetComponent<OtherPlayerManagerOffline>();
        p3Manager.Init(CardManager.Player3Hand, 3);

        EventCenter.AddListener(EventType.TestEvent, Test);
        EventCenter.AddListener(EventType.MatchExit, ReturnToLobby);
        EventCenter.AddListener(EventType.MatchReset, MatchReset);
        EventCenter.AddListener(EventType.DealCardOver, DealCardOver);

        CardManager.SetLastHand(CardSet.None);

        StartDealCard();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.TestEvent, Test);
        EventCenter.RemoveListener(EventType.MatchExit, ReturnToLobby);
        EventCenter.RemoveListener(EventType.MatchReset, MatchReset);
        EventCenter.RemoveListener(EventType.DealCardOver, DealCardOver);

    }

    private void Test()
    {
        MatchReset();
    }

    private void FixedUpdate()
    {
        if (turnEnabled)
            if (!turnStarted && turnEnded)
            {
                if (landlordPlayer == 0)
                    StartTurn(TurnType.GarbLandlord);
                else
                    StartTurn(TurnType.PlayCard);
            }
    }

    /// <summary>
    /// 开始一局新游戏
    /// </summary>
    private void MatchReset()
    {
        view.MatchReset();
        p1Manager.MatchReset();
        p2Manager.MatchReset();
        p3Manager.MatchReset();

        turnStarted = false;
        turnEnded = true;
        turnEnabled = false;
        passCount = 0;
        noGrabLandlordPlayerCount = 0;
        grabLandlordPlayerCount = 0;
        lastGrabLandlordPlayer = 0;
        startPlayer = lastWinner;
        currentPlayer = startPlayer;
        landlordPlayer = 0;

        CardManager.SetLastHand(CardSet.None);
        StartDealCard();
    }

    private void StartDealCard()
    {
        StartCoroutine(CardManager.DealCardCoroutine());
    }

    private void ReturnToLobby()
    {
        LoadingManager.LoadSceneByLoadingPanel(Constants.SceneName.Lobby);
    }

    private void DealCardOver()
    {
        turnEnabled = true;
    }
}
