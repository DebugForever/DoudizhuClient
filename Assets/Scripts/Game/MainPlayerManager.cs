using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class MainPlayerManager : MonoBehaviour
{
    private UIMainPlayer view;
    private GameManagerOffline gameManager;

    /// <summary>
    /// 当前手牌的引用，所有手牌存放在CardManager里
    /// 需要取出一个引用
    /// </summary>
    private CardHand cardHand;

    /// <summary>
    /// 自己也维护一个回合进行中状态的变量，
    /// 避免不在自己的回合时接收消息
    /// </summary>
    private bool isMyTurn = false;

    private int playerIndex;


    public void Init(CardHand cardHand)
    {
        this.cardHand = cardHand;
        this.playerIndex = 1;
    }

    private void Awake()
    {
        view = GetComponentInChildren<UIMainPlayer>();
        gameManager = GetComponentInParent<GameManagerOffline>();

        //注意每一个玩家出牌，所有玩家都会收到这个事件，但是只有1个玩家需要相应这个事件
        //这个没有办法优化，因为这是解耦的必要开销。除非每个玩家出牌都使用不同的事件（这显然不行）。
        EventCenter.AddListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.AddListener(EventType.PassTurn, PassTurn);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.RemoveListener(EventType.PassTurn, PassTurn);
    }

    private void Start()
    {
        view.AddTimeUpListener(EndTurn);
    }

    public void StartTurn()
    {
        view.StartTimer();
        isMyTurn = true;
    }

    private void EndTurn()
    {
        gameManager.EndTurn();
        view.StopTimer();
        isMyTurn = false;
    }

    private void PassTurn()
    {
        gameManager.EndTurnPass();
        view.StopTimer();
        isMyTurn = false;
    }

    /// <summary>
    /// 主玩家出牌
    /// </summary>
    private void MainPlayerPlayCard()
    {
        if (!isMyTurn)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "现在无法出牌");
            return;
        }

        Card[] cards = view.GetSelectedCards();
        if (cards.Length == 0)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "请选择要出的牌");
            return;
        }

        print(CardSet.GetCardSet(cards));
        CardSet cardSet = CardSet.GetCardSet(cards);
        if (cardSet.Type == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无效的组合");
            view.UnselectAllCard();
        }
        else if (!cardSet.IsGreaterThan(CardManager.LastHand))//牌型不同，或者没对方大
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无法大过上家");
            view.UnselectAllCard();
        }
        else //合法的出牌
        {
            view.RemoveSelectedCards();
            view.UnselectAllCard();

            EventCenter.BroadCast(EventType.PlayerPlayCard, CardSet.GetCardSet(cards));
            EndTurn();
        }
    }
}
