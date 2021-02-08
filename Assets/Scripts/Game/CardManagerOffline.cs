using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 管理所有卡牌的类
/// </summary>
class CardManagerOffline
{
    public CardHand mainPlayerHand = new CardHand();
    public CardHand player2Hand = new CardHand();
    public CardHand player3Hand = new CardHand();


    /// <summary>
    /// 保存所有54张卡牌，共用一份
    /// 只会打乱顺序，可以所有示例共用一份
    /// </summary>
    private static Card[] allCards;

    public CardManagerOffline()
    {
        if (allCards == null)
        {
            allCards = new Card[54];
            for (int i = 0; i < 54; i++)
            {
                allCards[i] = new Card(i);
            }
        }

    }

    /// <summary>
    /// 发牌并排序，三张底牌暂时没有实现
    /// </summary>
    public void DealCard()
    {
        MyTools.Shuffle(allCards);
        const int count = 17;

        mainPlayerHand.Clear();
        player2Hand.Clear();
        player3Hand.Clear();

        mainPlayerHand.AddCards(allCards.Take(count).ToArray());
        mainPlayerHand.Sort();
        EventCenter.BroadCast(EventType.MainPlayerAddCards, mainPlayerHand.GetCards());

        player2Hand.AddCards(allCards.Skip(count).Take(count).ToArray());
        player2Hand.Sort();
        EventCenter.BroadCast(EventType.Player2AddCards, player2Hand.GetCards());

        player3Hand.AddCards(allCards.Skip(count * 2).Take(count).ToArray());
        player3Hand.Sort();
        EventCenter.BroadCast(EventType.Player3AddCards, player3Hand.GetCards());

    }

}
