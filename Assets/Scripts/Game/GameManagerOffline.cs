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
    }

    private void Test()
    {
        cardManager.DealCard();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.TestEvent, Test);
        EventCenter.RemoveListener(EventType.PlayCard, MainPlayerPlayCard);
    }

    /// <summary>
    /// 主玩家出牌
    /// </summary>
    private void MainPlayerPlayCard()
    {
        Card[] cards = view.mainPlayerView.GetSelectedCards();
        print(CardSet.GetCardSetType(cards));

        //view.mainPlayerView.RemoveSelectedCards();
        view.mainPlayerView.UnselectAllCard();
    }
}
