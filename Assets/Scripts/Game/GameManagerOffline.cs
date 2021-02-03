using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerOffline : MonoBehaviour
{
    private GameView view;
    private CardManagerOffline cardManager;
    void Awake()
    {
        view = GetComponentInChildren<GameView>();
        cardManager = new CardManagerOffline();

        EventCenter.AddListener(EventType.TestEvent, Test);
        EventCenter.AddListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.AddListener<Card[]>(EventType.PlayerPlayCard, PlayerPlayCard);
        EventCenter.AddListener(EventType.PlayCardHint, PlayCardHint);
    }

    private void Test()
    {
        cardManager.DealCard();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.TestEvent, Test);
        EventCenter.RemoveListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.RemoveListener<Card[]>(EventType.PlayerPlayCard, PlayerPlayCard);
        EventCenter.RemoveListener(EventType.PlayCardHint, PlayCardHint);
    }

    /// <summary>
    /// 主玩家出牌
    /// </summary>
    private void MainPlayerPlayCard()
    {
        Card[] cards = view.mainPlayerView.GetSelectedCards();
        print(CardSet.GetCardSet(cards));
        if (CardSet.GetCardSetType(cards) == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无效的组合");
            view.mainPlayerView.UnselectAllCard();
        }
        else
        {
            view.mainPlayerView.RemoveSelectedCards();
            view.mainPlayerView.UnselectAllCard();

            EventCenter.BroadCast(EventType.PlayerPlayCard, cards);
        }
    }

    private void PlayCardHint()
    {

    }

    private void PlayerPlayCard(Card[] cards)
    {
        view.lastHandCards.ClearCards();
        view.lastHandCards.SetCards(cards);
    }
}
