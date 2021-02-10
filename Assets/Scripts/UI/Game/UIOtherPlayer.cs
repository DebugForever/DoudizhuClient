﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOtherPlayer : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text usernameText;
    private UnityEngine.UI.Text coinText;
    // === auto generated code end === 
    private Transform cardsTransform;
    private Image headIconImage;
    private UITimer timer;

    private int _coin;

    private void Awake()
    {
        // === auto generated code begin === 
        usernameText = transform.Find("InfoPanel/UsernamePanel/UsernameText").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("InfoPanel/CoinPanel/CoinText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        cardsTransform = transform.Find("Cards");
        headIconImage = transform.Find("InfoPanel/HeadIcon/HeadIconMask/HeadIconImage").GetComponent<Image>();
        timer = transform.Find("Timer").GetComponent<UITimer>();
    }

    public string username
    {
        get => usernameText.text; set => usernameText.text = value;
    }

    public int coin
    {
        get => _coin; set { _coin = value; coinText.text = _coin.ToString(); }
    }

    public Sprite headIcon
    {
        get => headIconImage.sprite;
        set => headIconImage.sprite = value;
    }

    public void AddCard(Card card)
    {
        GameObject go = Instantiate(ResourceManager.GetSingleCardPrefab());
        go.transform.SetParent(cardsTransform);
        SingleCard singleCard = go.GetComponent<SingleCard>();
        singleCard.card = card;
        //singleCard.FaceDown();

    }

    public void AddCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

    public void RemoveCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            int index = card.handId;
            Destroy(cardsTransform.GetChild(index).gameObject);
        }
    }

    public void ClearCards()
    {
        MyTools.DestroyAllChild(cardsTransform);
    }

    public void StartTimer()
    {
        timer.StartTimer(60);
    }

    public void StopTimer()
    {
        timer.StopTimer();
    }

    public void AddTimeUpListener(Action action)
    {
        timer.TimeUp += action;
    }
}
