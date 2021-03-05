using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnder3Cards : HideablePanel
{
    private SingleCard[] underCards = new SingleCard[3];

    private void Awake()
    {
        underCards[0] = transform.Find("UnderCard1").GetComponent<SingleCard>();
        underCards[1] = transform.Find("UnderCard2").GetComponent<SingleCard>();
        underCards[2] = transform.Find("UnderCard3").GetComponent<SingleCard>();

        EventCenter.AddListener<Card[]>(EventType.SetUnder3Cards, SetUnderCards);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<Card[]>(EventType.SetUnder3Cards, SetUnderCards);
    }

    public void MatchReset()
    {
        CardsFaceDown();
    }

    public void CardsFaceDown()
    {
        foreach (SingleCard card in underCards)
        {
            card.FaceDown();
        }
    }

    public void CardsFaceUp()
    {
        foreach (SingleCard card in underCards)
        {
            card.FaceUp();
        }
    }

    public void SetUnderCards(Card[] cards)
    {
        for (int i = 0; i < 3; i++)
        {
            underCards[i].card = cards[i];
        }
        CardsFaceDown();
    }
}
