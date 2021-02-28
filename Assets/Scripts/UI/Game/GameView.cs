using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//view指视图，就是MVC中的View层
public class GameView : MonoBehaviour
{
    public UIMainPlayer mainPlayerView { get; private set; }
    public UIOtherPlayer player2View { get; private set; }
    public UIOtherPlayer player3View { get; private set; }
    public UIResultPanel resultPanel { get; private set; }
    public UIUnder3Cards under3Cards { get; private set; }


    private void Awake()
    {
        mainPlayerView = transform.Find("MainPlayer").GetComponent<UIMainPlayer>();
        player2View = transform.Find("Player2").GetComponent<UIOtherPlayer>();
        player3View = transform.Find("Player3").GetComponent<UIOtherPlayer>();
        resultPanel = transform.Find("ResultPanel").GetComponent<UIResultPanel>();
        under3Cards = transform.Find("Under3Cards").GetComponent<UIUnder3Cards>();

        EventCenter.AddListener<Card[]>(EventType.MainPlayerAddCards, MainPlayerAddCards);
        EventCenter.AddListener<Card[]>(EventType.Player2AddCards, Player2AddCards);
        EventCenter.AddListener<Card[]>(EventType.Player3AddCards, Player3AddCards);
        EventCenter.AddListener<Card[]>(EventType.MainPlayerRemoveCards, MainPlayerRemoveCards);
        EventCenter.AddListener<Card[]>(EventType.Player2RemoveCards, Player2RemoveCards);
        EventCenter.AddListener<Card[]>(EventType.Player3RemoveCards, Player3RemoveCards);
    }



    private void Start()
    {
        Init();
        resultPanel.HideNoAnim();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<Card[]>(EventType.MainPlayerAddCards, MainPlayerAddCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player2AddCards, Player2AddCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player3AddCards, Player3AddCards);
        EventCenter.RemoveListener<Card[]>(EventType.MainPlayerRemoveCards, MainPlayerRemoveCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player2RemoveCards, Player2RemoveCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player3RemoveCards, Player3RemoveCards);
    }

    private void Init()
    {
        mainPlayerView.ClearCards();
        player2View.ClearCards();
        player3View.ClearCards();
    }

    public void MatchReset()
    {
        mainPlayerView.MatchReset();
        player2View.MatchReset();
        player3View.MatchReset();
        under3Cards.MatchReset();
    }

    private void MainPlayerAddCards(Card[] cards)
    {
        mainPlayerView.AddCards(cards);
    }

    private void Player2AddCards(Card[] cards)
    {
        player2View.AddCards(cards);
    }

    private void Player3AddCards(Card[] cards)
    {
        player3View.AddCards(cards);
    }

    private void MainPlayerRemoveCards(Card[] cards)
    {
        mainPlayerView.RemoveCards(cards);
    }


    private void Player2RemoveCards(Card[] cards)
    {
        player2View.RemoveCards(cards);
    }

    private void Player3RemoveCards(Card[] cards)
    {
        player3View.RemoveCards(cards);
    }


}
