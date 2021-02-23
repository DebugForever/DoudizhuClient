using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBase : MonoBehaviour
{
    // === auto generated code begin === 
    protected UnityEngine.UI.Text usernameText;
    protected UnityEngine.UI.Text coinText;
    // === auto generated code end === 
    protected Transform cardsTransform;
    protected Image headIconImage;
    protected UITimer timer;
    protected int _coin;

    protected virtual void Awake()
    {
        
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

    public virtual void AddCard(Card card)
    {
        GameObject go = Instantiate(ResourceManager.GetSingleCardPrefab());
        go.transform.SetParent(cardsTransform);
        SingleCard singleCard = go.GetComponent<SingleCard>();
        singleCard.card = card;
        //singleCard.FaceDown();
    }

    public virtual void AddCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

    public virtual void RemoveCards(Card[] cards)
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
