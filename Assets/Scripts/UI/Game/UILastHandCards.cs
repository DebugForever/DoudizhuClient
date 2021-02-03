using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILastHandCards : MonoBehaviour
{
    public void SetCards(Card[] cards)
    {
        ClearCards();
        AddCards(cards);
    }

    private void AddCard(Card card)
    {
        GameObject go = Instantiate(ResourceManager.GetSingleCardPrefab());
        go.transform.SetParent(transform);
        SingleCard singleCard = go.GetComponent<SingleCard>();
        singleCard.card = card;
    }

    private void AddCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

    public void ClearCards()
    {
        MyTools.DestroyAllChild(transform);
    }
}
