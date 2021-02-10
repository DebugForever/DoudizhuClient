using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class OtherPlayerManager : MonoBehaviour
{
    private UIOtherPlayer view;
    private GameManagerOffline gameManager;

    /// <summary>
    /// 当前手牌的引用，所有手牌存放在CardManager里
    /// 需要取出一个引用
    /// </summary>
    private CardHand cardHand;

    private bool isMyTurn = false;

    /// <summary>
    /// 这个玩家的编号
    /// </summary>
    /// <remarks>从2开始，因为1是主玩家。</remarks>
    private int playerIndex;

    private AIPlayer ai;

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
    /// improve:需要传的参数有点多，考虑怎么解耦
    public void Init(CardHand cardHand, int playerIndex)
    {
        this.cardHand = cardHand;
        this.playerIndex = playerIndex;
        this.ai = new AIPlayer(cardHand);
    }


    private void Awake()
    {
        view = GetComponentInChildren<UIOtherPlayer>();
        gameManager = GetComponentInParent<GameManagerOffline>();
    }

    private void Start()
    {
        view.AddTimeUpListener(EndTurn);
    }

    public void StartTurn()
    {
        Debug.LogFormat("player{0} turn started", playerIndex);
        view.StartTimer();
        isMyTurn = true;
        StartCoroutine(WaitPlayCard());
    }

    private void EndTurn()
    {
        view.StopTimer();
        gameManager.EndTurn();
        isMyTurn = false;
    }

    private void PassTurn()
    {
        Debug.LogFormat("player{0} turn passed", playerIndex);
        view.StopTimer();
        gameManager.EndTurnPass();
        isMyTurn = false;
    }

    private IEnumerator WaitPlayCard()
    {
        if (IsAI)
        {
            yield return new WaitForSeconds(1.0f);
            CardSet result = ai.PlayCard(CardManager.LastHand);
            print("AI CardSet:" + result);
            if (result.Type == CardSetType.Invalid || result.Type == CardSetType.None)
            {
                PassTurn();
                yield break;
            }
            cardHand.RemoveCards(result.Cards);
            view.RemoveCards(result.Cards);
            EventCenter.BroadCast(EventType.PlayerPlayCard, result);
            EndTurn();
        }
        else
        {
            //玩家控制，这里没有，直接不出。
            //或者等待回合结束也行
            yield return new WaitForSeconds(1.0f);
            PassTurn();
        }

    }
}
