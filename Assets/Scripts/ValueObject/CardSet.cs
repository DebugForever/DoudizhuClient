using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 出牌的类型
/// </summary>
public enum CardSetType
{
    ///<summary>无牌型</summary>
    ///<remarks>没人要牌或者开始时会出现</remarks>
    None,
    ///<summary>王炸</summary>
    JokerBomb,
    ///<summary>炸</summary>
    Bomb,
    ///<summary>单张</summary>
    Single,
    ///<summary>对子</summary>
    Pair,
    ///<summary>三张</summary>
    Triple,
    ///<summary>三带一</summary>
    TripleWithOne,
    ///<summary>三带二</summary>
    TripleWithPair,
    ///<summary>顺子</summary>
    Straight,
    ///<summary>双顺（连对）</summary>
    DoubleStraight,
    ///<summary>三顺</summary>
    TripleStraight,
    ///<summary>三顺带1（飞机）</summary>
    TripleStraightWithOne,
    ///<summary>三顺带2（飞机带对子）</summary>
    TripleStraightWithPair,
    ///<summary>四带二</summary>
    QuadraWithTwo,
    ///<summary>四带两对</summary>
    QuadraWithTwoPairs,
}

/// <summary>
/// 牌型，又可以说是一手牌，每次出牌使用这个类
/// </summary>
public class CardSet : IComparable<CardSet>
{
    CardSetType setType;

    /// <summary>
    /// 权值，表示这种牌的大小
    /// </summary>
    /// <remarks>这里使用第一比较关键字里最大的牌</remarks>
    /// <example>顺子34567的权值是7，三带二33399的权值是3</example>
    int keyNumber;

    ///// <summary>
    ///// 副权值，第二比较关键字，暂时用不上
    ///// </summary>
    //int subKeyNumber;

    /// <summary>
    /// 包含的牌
    /// </summary>
    Card[] cards;

    /// <summary>
    /// 排序比较，在类型相同时可以用于大小比较
    /// </summary>
    public int CompareTo(CardSet other)
    {
        return setType == other.setType ? keyNumber.CompareTo(other.keyNumber) : setType.CompareTo(other.setType);
    }



    public static CardSetType GetCardSetType(Card[] cards)
    {
        int[] cntArr = new int[16];//不使用[0]
        foreach (Card card in cards)
        {
            if (card.number < 1 || card.number > 15)
                continue;
            cntArr[card.number] += 1;
        }

        int differentCards = 0;
        for (int i = 0; i < 16; i++)
        {
            if (cntArr[i] > 0)
                differentCards += 1;
        }

        Card[] cardsSorted = cards.Clone() as Card[];
        Array.Sort(cardsSorted);

        if (IsSingle())
            return CardSetType.Single;
        else if (IsPair())
            return CardSetType.Pair;
        else if (IsTriple())
            return CardSetType.Triple;
        else if (IsBomb())
            return CardSetType.Bomb;
        else if (IsJokerBomb())
            return CardSetType.JokerBomb;



        bool IsSingle()
        {
            return cardsSorted.Length == 1;
        }

        bool IsPair()
        {
            return cardsSorted.Length == 2 && differentCards == 1;
        }

        bool IsTriple()
        {
            return cardsSorted.Length == 3 && differentCards == 1;
        }

        bool IsBomb()
        {
            return cardsSorted.Length == 4 && differentCards == 1;
        }

        bool IsJokerBomb()
        {
            return cardsSorted.Length == 2
                && cardsSorted[0].type == CardType.JokerBlack
                && cardsSorted[1].type == CardType.JokerRed;
        }

        bool IsTripleWithOne()
        {

        }
    }

}

