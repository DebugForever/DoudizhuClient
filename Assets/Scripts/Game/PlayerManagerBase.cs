using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class PlayerManagerBase : MonoBehaviour
{
    protected UIPlayerBase view;
    protected GameManagerOffline gameManager;

    /// <summary>
    /// 当前手牌的引用，所有手牌存放在CardManager里
    /// 需要取出一个引用
    /// </summary>
    protected CardHand cardHand;

    protected bool isMyTurn = false;

    /// <summary>
    /// 这个玩家的编号
    /// </summary>
    /// <remarks>从2开始，因为1是主玩家。</remarks>
    protected int playerIndex;

    protected AIPlayer ai;

    /// <summary>
    /// 这个玩家目前是否由电脑操控
    /// 可随时更改
    /// </summary>
    public bool IsAI { get; set; } = true;

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


    protected virtual void Awake()
    {

    }

    protected void Start()
    {
        view.AddTimeUpListener(EndTurn);
    }

    public virtual void StartTurn()
    {
        Debug.LogFormat("player{0} turn started", playerIndex);
        view.StartTimer();
        isMyTurn = true;
    }

    protected virtual void EndTurn()
    {
        view.StopTimer();
        gameManager.EndTurn();
        isMyTurn = false;
    }

    protected virtual void PassTurn()
    {
        Debug.LogFormat("player{0} turn passed", playerIndex);
        view.StopTimer();
        gameManager.EndTurnPass();
        isMyTurn = false;
    }
}
