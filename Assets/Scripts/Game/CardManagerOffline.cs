using System.Collections;
using System.Linq;
using UnityEngine;


/// <summary>
/// 管理所有卡牌的类，因为很多地方需要访问，直接做成单例了
/// </summary>
public static class CardManagerOffline
{
    public static CardHand MainPlayerHand { get; private set; } = new CardHand();
    public static CardHand Player2Hand { get; private set; } = new CardHand();
    public static CardHand Player3Hand { get; private set; } = new CardHand();
    public static CardSet LastHand { get; private set; } = CardSet.None;

    /// <summary>
    /// 保存所有54张卡牌，共用一份
    /// 只会打乱顺序，可以所有实例共用一份
    /// </summary>
    private static readonly Card[] allCards;

    public static void SetLastHand(CardSet cardSet)
    {
        LastHand = cardSet;
    }

    static CardManagerOffline()
    {
        allCards = new Card[54];
        for (int i = 0; i < 54; i++)
        {
            allCards[i] = new Card(i);
        }

        //没有RemoveListener，因为静态类不能析构
        EventCenter.AddListener<CardSet>(EventType.PlayerPlayCard, SetLastHand);
    }

    /// <summary>
    /// 发牌并排序，三张底牌暂时没有实现
    /// </summary>
    public static IEnumerator DealCardCoroutine()
    {
        MyTools.Shuffle(allCards);
        const int count = 17;

        CleanCards();//Destroy需要一帧来完成，所以不能立即发牌（或者把代码改成DestroyzImmediately）
        yield return new WaitForEndOfFrame();


        MainPlayerHand.AddCards(allCards.Take(count).ToArray());
        MainPlayerHand.Sort();

        Player2Hand.AddCards(allCards.Skip(count).Take(count).ToArray());
        Player2Hand.Sort();

        Player3Hand.AddCards(allCards.Skip(count * 2).Take(count).ToArray());
        Player3Hand.Sort();


        Card[] mainPlayerCards = MainPlayerHand.GetCards();
        Card[] Player2Cards = Player2Hand.GetCards();
        Card[] Player3Cards = Player3Hand.GetCards();

        const int takeCount = 4;
        for (int i = 0; i < mainPlayerCards.Length; i += takeCount)
        {
            EventCenter.BroadCast(EventType.MainPlayerAddCards, mainPlayerCards.Skip(i).Take(takeCount).ToArray());
            EventCenter.BroadCast(EventType.Player2AddCards, Player2Cards.Skip(i).Take(takeCount).ToArray());
            EventCenter.BroadCast(EventType.Player3AddCards, Player3Cards.Skip(i).Take(takeCount).ToArray());
            yield return new WaitForSeconds(0.2f);
        }

    }

    private static void CleanCards()
    {
        EventCenter.BroadCast(EventType.MainPlayerRemoveCards, MainPlayerHand.GetCards());
        EventCenter.BroadCast(EventType.Player2RemoveCards, Player2Hand.GetCards());
        EventCenter.BroadCast(EventType.Player3RemoveCards, Player3Hand.GetCards());
        MainPlayerHand.Clear();
        Player2Hand.Clear();
        Player3Hand.Clear();
    }
}
