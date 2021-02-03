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
    public List<Card> mainPlayerCards = new List<Card>();
    private List<Card> player2Cards = new List<Card>();
    private List<Card> player3Cards = new List<Card>();


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

        mainPlayerCards.Clear();
        player2Cards.Clear();
        player3Cards.Clear();

        //mainPlayerCards.AddRange(allCards.Take(count));
        mainPlayerCards.AddRange(allCards);
        mainPlayerCards.Sort((a, b) => -a.CompareTo(b));//从大到小排序
        EventCenter.BroadCast(EventType.MainPlayerAddCards, mainPlayerCards.ToArray());

        player2Cards.AddRange(allCards.Skip(count).Take(count));
        player2Cards.Sort();
        EventCenter.BroadCast(EventType.Player2AddCards, player2Cards.ToArray());

        player3Cards.AddRange(allCards.Skip(count * 2).Take(count));
        player3Cards.Sort();
        EventCenter.BroadCast(EventType.Player3AddCards, player3Cards.ToArray());

    }

}
