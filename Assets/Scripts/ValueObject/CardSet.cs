using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 出牌的类型
/// </summary>
public enum CardSetType
{
    /// <summary>
    /// 不合法的牌型
    /// </summary>
    Invalid = -1,
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
        Dictionary<int, int> cntDict = new Dictionary<int, int>();
        foreach (Card card in cards)
        {
            if (cntDict.ContainsKey((int)card.weight))
                cntDict[(int)card.weight] += 1;
            else
                cntDict.Add((int)card.weight, 1);
        }
        int differentCards = cntDict.Count;

        ///<summary>手牌结构，按照个数排序</summary>
        List<(int cnt, int weight)> handData = new List<(int cnt, int weight)>();//ValueTuple语法糖
        foreach (var item in cntDict)
        {
            handData.Add((cnt: item.Value, weight: item.Key));
        }
        handData.Sort();

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
        else if (IsTripleWithOne())
            return CardSetType.TripleWithOne;
        else if (IsTripleWithPair())
            return CardSetType.TripleWithPair;
        else if (IsQuadraWithTwo())
            return CardSetType.QuadraWithTwo;
        else if (IsQuadraWithTwoPairs())
            return CardSetType.QuadraWithTwoPairs;
        else if (IsStraight())
            return CardSetType.Straight;
        else if (IsDoubleStraight())
            return CardSetType.DoubleStraight;
        else if (IsTripleStraight())
            return CardSetType.TripleStraight;
        else if (IsTripleStraightWithOne())
            return CardSetType.TripleStraightWithOne;
        else if (IsTripleStraightWithPair())
            return CardSetType.TripleStraightWithPair;

        return CardSetType.Invalid;

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
            return handData.Count == 2 && handData[0].cnt == 1 && handData[1].cnt == 3;
        }

        bool IsTripleWithPair()
        {
            return handData.Count == 2 && handData[0].cnt == 2 && handData[1].cnt == 3;
        }

        bool IsQuadraWithTwo()
        {
            return handData.Count == 3 && handData[0].cnt == 1 && handData[1].cnt == 1 && handData[2].cnt == 4;
        }

        bool IsQuadraWithTwoPairs()
        {
            return handData.Count == 3 && handData[0].cnt == 2 && handData[1].cnt == 2 && handData[2].cnt == 4;
        }

        bool IsStraight()
        {
            //顺子至少5张牌
            if (handData.Count < 5)
                return false;

            //顺子只能包含3~A
            if (cardsSorted[0].weight < CardWeight.w3 || cardsSorted[cardsSorted.Length - 1].weight > CardWeight.wA)
                return false;

            //顺子每张应该连续
            for (int i = 1; i < handData.Count; i++)
            {
                if (handData[i].weight != handData[i - 1].weight + 1)
                    return false;
            }

            //顺子每样只有一张
            //handData按张数排序
            if (handData[0].cnt != 1 || handData[handData.Count - 1].cnt != 1)
                return false;

            return true;
        }

        //和上面的顺子基本一样，代码是复制改的
        bool IsDoubleStraight()
        {
            if (handData.Count < 3)
                return false;

            if (cardsSorted[0].weight < CardWeight.w3 || cardsSorted[cardsSorted.Length - 1].weight > CardWeight.wA)
                return false;

            for (int i = 1; i < handData.Count; i++)
            {
                if (handData[i].weight != handData[i - 1].weight + 1)
                    return false;
            }

            if (handData[0].cnt != 2 || handData[handData.Count - 1].cnt != 2)
                return false;

            return true;
        }

        //和上面的顺子基本一样，代码是复制改的
        bool IsTripleStraight()
        {
            if (handData.Count < 3)
                return false;

            if (cardsSorted[0].weight < CardWeight.w3 || cardsSorted[cardsSorted.Length - 1].weight > CardWeight.wA)
                return false;

            for (int i = 1; i < handData.Count; i++)
            {
                if (handData[i].weight != handData[i - 1].weight + 1)
                    return false;
            }

            if (handData[0].cnt != 3 || handData[handData.Count - 1].cnt != 3)
                return false;

            return true;
        }

        bool IsTripleStraightWithOne()
        {
            //飞机两个起飞
            if (handData.Count < 4)
                return false;

            //种类数要是偶数
            if (handData.Count % 2 != 0)
                return false;

            int halfCount = handData.Count / 2;

            //前半，是被带的
            for (int i = 0; i < halfCount; i++)
            {
                if (handData[i].cnt != 1)
                    return false;
            }

            //后半，是带的，需要3个一组
            for (int i = halfCount; i < handData.Count; i++)
            {
                if (handData[i].cnt != 3)
                    return false;

                //三顺部分需要连续
                if (i > halfCount && handData[i - 1].weight + 1 != handData[i].weight)
                    return false;
            }

            return true;
        }

        //和上面的飞机基本一样，代码是复制改的
        bool IsTripleStraightWithPair()
        {
            //飞机两个起飞
            if (handData.Count < 4)
                return false;

            //种类数要是偶数
            if (handData.Count % 2 != 0)
                return false;

            int halfCount = handData.Count / 2;

            //前半，是被带的
            for (int i = 0; i < halfCount; i++)
            {
                if (handData[i].cnt != 2)
                    return false;
            }

            //后半，是带的，需要3个一组
            for (int i = halfCount; i < handData.Count; i++)
            {
                if (handData[i].cnt != 3)
                    return false;

                //三顺部分需要连续
                if (i > halfCount && handData[i - 1].weight + 1 != handData[i].weight)
                    return false;
            }

            return true;
        }

    }

}

