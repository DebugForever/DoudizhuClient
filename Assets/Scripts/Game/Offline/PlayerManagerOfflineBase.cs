using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码
using ServerProtocol.SharedCode;
public enum TurnType
{
    PlayCard,
    GarbLandlord
}

public class PlayerManagerOfflineBase : MonoBehaviour
{
    [NonSerialized] protected UIPlayerBase view;//[NonSerialized]用于解决unity重复序列化报错，但是不知道能不能解决。
    protected GameManagerOffline gameManager;

    /// <summary>
    /// 当前手牌的引用，所有手牌存放在CardManager里
    /// 需要取出一个引用
    /// </summary>
    protected CardHand cardHand;

    protected bool isMyTurn = false;

    protected TurnType turnType;

    protected bool isLandLord = false;

    /// <summary>
    /// 这个玩家的编号
    /// </summary>
    /// <remarks>从2开始，因为1是主玩家。</remarks>
    protected int playerIndex;

    protected AIPlayer ai;

    [SerializeField]
    private bool _isAI = true;

    /// <summary>
    /// 这个玩家目前是否由电脑操控
    /// 可随时更改
    /// </summary>
    public bool IsAI { get => _isAI; set => _isAI = value; }

    /// <summary>
    /// 这个类需要在外部初始化后才能使用
    /// </summary>
    /// <param name="cardHand">CardManager的某个手牌类</param>
    /// <param name="playerIndex">玩家编号</param>
    /// <remarks>不是构造函数的形式是因为构造是Unity进行的</remarks>
    public void Init(CardHand cardHand, int playerIndex)
    {
        this.cardHand = cardHand;
        this.playerIndex = playerIndex;
        this.ai = new AIPlayer(cardHand);
    }

    public virtual void MatchReset()
    {
        cardHand.Clear();
    }

    protected virtual void Awake()
    {

    }

    protected void Start()
    {
        view.AddTimeUpListener(DefaultEndTurn);
    }

    public virtual void StartTurn(TurnType turnType)
    {
        Debug.LogFormat("player{0} turn started", playerIndex);
        view.StartTimer();
        isMyTurn = true;
        this.turnType = turnType;
        view.HideStatusText();
    }

    protected virtual void DefaultEndTurn()
    {
        if (turnType == TurnType.GarbLandlord)
            EndTurnGrabLandlord(false);
        else if (turnType == TurnType.PlayCard)
            PassTurn();
    }

    protected virtual void EndTurnPlayCard()
    {
        view.StopTimer();
        gameManager.EndTurnPlayCard();
        isMyTurn = false;
    }

    protected virtual void PassTurn()
    {
        Debug.LogFormat("player{0} turn passed", playerIndex);
        view.StopTimer();
        gameManager.EndTurnPass();
        isMyTurn = false;
        view.SetStatusText("不出");
        view.ShowStatusText();
        view.lastHandCards.ClearCards();
    }

    protected virtual void EndTurnGrabLandlord(bool isGrab)
    {
        view.StopTimer();
        gameManager.EndTurnGrabLandlord(isGrab);
        isMyTurn = false;
        if (isGrab)
        {
            view.SetStatusText("抢地主");
            Debug.LogFormat("player{0} GrabLandlord", playerIndex);
        }
        else
        {
            view.SetStatusText("不抢");
            Debug.LogFormat("player{0} NOT GrabLandlord", playerIndex);
        }
        view.ShowStatusText();
    }

    public int GetRemainCardCount()
    {
        return cardHand.CardCount;
    }

    public void BecomeLandlord()
    {
        isLandLord = true;
    }

    protected virtual void PlayCard(CardSet cardSet)
    {
        cardHand.RemoveCards(cardSet.Cards);
        view.RemoveCards(cardSet.Cards);
        view.lastHandCards.SetCards(cardSet.Cards);
        EventCenter.BroadCast(EventType.PlayerPlayCard, cardSet);
        EndTurnPlayCard();
    }
}
