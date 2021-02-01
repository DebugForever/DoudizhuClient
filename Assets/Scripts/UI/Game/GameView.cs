using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//view指视图，就是MVC中的View层
public class GameView : MonoBehaviour
{
    public UIMainPlayer mainPlayerView { get; private set; }
    public UIOtherPlayer player2View { get; private set; }
    public UIOtherPlayer player3View { get; private set; }
    private void Awake()
    {
        mainPlayerView = transform.Find("MainPlayer").GetComponent<UIMainPlayer>();
        player2View = transform.Find("Player2").GetComponent<UIOtherPlayer>();
        player3View = transform.Find("Player3").GetComponent<UIOtherPlayer>();

        EventCenter.AddListener<Card[]>(EventType.MainPlayerAddCards, MainPlayerAddCards);
        EventCenter.AddListener<Card[]>(EventType.Player2AddCards, Player2AddCards);
        EventCenter.AddListener<Card[]>(EventType.Player3AddCards, Player3AddCards);

    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
    }

    private void Init()
    {
        mainPlayerView.ClearCards();
        player2View.ClearCards();
        player3View.ClearCards();
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

}
