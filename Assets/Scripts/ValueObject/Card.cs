using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌的花色类型，包括桃心梅方和大小王
/// </summary>
public enum CardType
{
    JokerRed,//大王
    JokerBlack,//小王
    Spade,//黑桃♠
    Heart,//红心♥
    Club,//梅花♣
    Diamond,//方块♦
}

/// <summary>
/// 卡牌的颜色类型，可以通过花色决定
/// </summary>
public enum CardColor
{
    NoColor,
    Black,
    Red,
}

/// <summary>
/// 卡牌类，表示一张卡牌
/// </summary>
public class Card : IComparable<Card>
{
    public CardType type;
    public int number;

    private readonly int[] sortOrder = new int[16] { 0, 12, 13, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 15 };

    public CardColor color
    {
        get
        {
            switch (this.type)
            {
                case CardType.Spade:
                case CardType.Club:
                case CardType.JokerBlack:
                    return CardColor.Black;
                case CardType.Heart:
                case CardType.Diamond:
                case CardType.JokerRed:
                    return CardColor.Red;
                default:
                    return CardColor.NoColor;
            }
        }
    }

    public Card(CardType type, int number)
    {
        this.type = type;
        this.number = number;
        if (type == CardType.JokerBlack)
            this.number = 14;
        else if (type == CardType.JokerRed)
            this.number = 15;
        else if (number < 1 || number > 13)
            throw new ArgumentOutOfRangeException("非大小王的卡牌数字不在1~13范围内");
    }

    /// <summary>
    /// 通过ID生成一张卡牌
    /// </summary>
    /// <remarks>
    /// 卡牌id：小王52，大王53，
    /// 黑桃A~K 0~12
    /// 红心A~K 13~25
    /// 梅花A~K 26~38
    /// 方片A~K 39~51
    /// </remarks>
    public Card(int id)
    {

        if (id == 52)
        {
            this.type = CardType.JokerBlack;
            this.number = 14;
        }
        else if (id == 53)
        {
            this.type = CardType.JokerRed;
            this.number = 15;
        }
        else
        {
            this.type = (CardType)(id / 13);
            this.number = id % 13 + 1;
        }
    }

    //因为大王小王的数字已经定义了是15和14，
    //所以这里可以直接用number比较
    //这个比较函数用于排序
    public int CompareTo(Card other)
    {
        return number == other.number ? type.CompareTo(other.type) : -sortOrder[number].CompareTo(sortOrder[other.number]);
    }

    public Sprite GetSprite()
    {
        return ResourceManager.GetCard(type, number);
    }

    public override string ToString()
    {
        if (type == CardType.JokerBlack || type == CardType.JokerRed)
            return type.ToString();
        else
            return type.ToString() + number.ToString();
    }
}
