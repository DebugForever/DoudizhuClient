using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPlayer : UIPlayerBase
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text ratioText;
    // === auto generated code end === 
    private UIFunctionButtons functionButtons;
    private List<MainPlayerCard> cards = new List<MainPlayerCard>();

    private int _ratio;

    protected override void Awake()
    {
        // === auto generated code begin === 
        usernameText = transform.Find("UsernamePanel/UsernameText").GetComponent<UnityEngine.UI.Text>();
        ratioText = transform.Find("InfoPanel/RatioPanel/Text").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("InfoPanel/CoinPanel/Text").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        cardsTransform = transform.Find("Cards");
        headIconImage = transform.Find("HeadIcon/HeadIconMask/HeadIconImage").GetComponent<Image>();
        statusText = transform.Find("StatusText").GetComponent<Text>();
        timer = transform.Find("Timer").GetComponent<UITimer>();
        functionButtons = transform.Find("FunctionButtons").GetComponent<UIFunctionButtons>();
    }

    protected override void Start()
    {
        base.Start();
        HideButtons();
    }

    public override void MatchReset()
    {
        base.MatchReset();
        HideButtons();
    }

    public override void AddCard(Card card)
    {
        GameObject go = Instantiate(ResourceManager.GetMainPlayerCardPrefab());
        go.transform.SetParent(cardsTransform);
        MainPlayerCard mainPlayerCard = go.GetComponent<MainPlayerCard>();
        mainPlayerCard.card = card;
        cards.Add(mainPlayerCard);
    }

    public override void RemoveCards(Card[] cards)
    {
        Card[] cardsClone = cards.Clone() as Card[];
        Array.Sort(cardsClone, (a, b) => a.handId.CompareTo(b.handId));//handid必须升序才能使用以下算法。
        for (int i = 0; i < cards.Length; i++)
        {
            Card card = cards[i];
            int index = card.handId;
            this.cards.RemoveAt(index - i);//删除会导致元素位置改变，所以要调整index
            Destroy(cardsTransform.GetChild(index).gameObject);//Destroy不会立即执行，所以不用调整index
        }
    }

    public void SelectCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            int index = card.handId;
            this.cards[index].Select();
        }
    }

    public int[] GetSelectedCardsId()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            MainPlayerCard card = cards[i];
            if (card.selected)
                result.Add(i);
        }
        return result.ToArray();
    }

    public Card[] GetSelectedCards()
    {
        List<Card> result = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            MainPlayerCard card = cards[i];
            if (card.selected)
                result.Add(card.card);
        }
        return result.ToArray();
    }

    public void RemoveSelectedCards()
    {
        int[] removeIndexs = GetSelectedCardsId();
        for (int i = 0; i < removeIndexs.Length; i++)
        {
            cards.RemoveAt(removeIndexs[i] - i);//删除会导致元素位置改变，所以要调整index
            Destroy(cardsTransform.GetChild(removeIndexs[i]).gameObject);//Destroy不会立即执行，所以不用调整index
        }

    }

    public void UnselectAllCard()
    {
        foreach (MainPlayerCard mainPlayerCard in cards)
        {
            mainPlayerCard.Unselect();
        }
    }

    public void EnableButtons()
    {
        functionButtons.EnableButtons();
    }

    public void DisableButtons()
    {
        functionButtons.DisableButtons();
    }

    public void ShowButtons()
    {
        functionButtons.ShowNoAnim();
    }

    public void HideButtons()
    {
        functionButtons.HideNoAnim();
    }

    public void ButtonsSwitchPlayCard()
    {
        functionButtons.SwitchPlayCard();
    }

    public void ButtonsSwitchGrabLandlord()
    {
        functionButtons.SwitchGrabLandlord();
    }
}
