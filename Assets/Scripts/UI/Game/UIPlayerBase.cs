using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ServerProtocol.Dto;
using ServerProtocol.SharedCode;
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
    protected Text statusText;
    public UILastHandCards lastHandCards { get; protected set; }

    protected virtual void Awake() //在Awake里完成变量初始化
    {

    }

    protected virtual void Start()
    {
        HideStatusText();
    }

    public virtual void MatchReset()
    {
        HideStatusText();
        StopTimer();
        ClearCards();
        lastHandCards.ClearCards();
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
        singleCard.FaceDown();
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

    /// <summary>
    /// 移除手牌中最右边的一些卡牌，至多清空手牌。
    /// </summary>
    /// <param name="count">要移除的卡牌数量</param>
    public virtual void RemoveRightCards(int count)
    {
        int childCount = cardsTransform.childCount;
        int startIndex = Math.Max(childCount - count + 1, 0);
        for (int i = startIndex; i < childCount; i++)
        {
            Destroy(cardsTransform.GetChild(i).gameObject);
        }
    }

    public virtual void ClearCards()
    {
        MyTools.DestroyAllChild(cardsTransform);
    }

    public void StartTimer()
    {
        timer.StartTimer(SharedConstants.TurnDuration / 1000);
    }

    public void StopTimer()
    {
        timer.StopTimer();
    }

    public void AddTimeUpListener(Action action)
    {
        timer.TimeUp += action;
    }

    public void ShowStatusText()
    {
        statusText.gameObject.SetActive(true);
    }

    public void HideStatusText()
    {
        statusText.gameObject.SetActive(false);
    }

    public void SetStatusText(string str)
    {
        statusText.text = str;
    }

    public void ResetPlayerUI(MatchRoomUserInfoDto dto)
    {
        UserInfoDto userInfo = dto.userInfo;
        username = userInfo.username;
        headIcon = ResourceManager.GetHeadIcon(userInfo.iconName);
        coin = userInfo.coin;
        if (dto.ready)
        {
            statusText.text = "已准备";
            ShowStatusText();
        }
        else
        {
            HideStatusText();
        }
    }

    public void AddFaceDownCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(ResourceManager.GetSingleCardPrefab());
            go.transform.SetParent(cardsTransform);
            SingleCard singleCard = go.GetComponent<SingleCard>();
            singleCard.FaceDown();
        }
    }
}
