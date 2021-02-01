﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌的花色类型，包括桃心梅方和大小王
/// </summary>
public enum CardType
{
    Spade,//黑桃♠
    Heart,//红心♥
    Club,//梅花♣
    Diamond,//方块♦
    JokerBlack,//小王
    JokerRed,//大王
}

public enum CardWeight
{
    wMin,
    w3,
    w4,
    w5,
    w6,
    w7,
    w8,
    w9,
    w10,
    wJ,
    wQ,
    wK,
    wA,
    w2,
    wJokerBlack,
    wJokerRed,
    wMax,
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

    /// <summary>
    /// 大小，即比较权重
    /// </summary>
    public CardWeight weight
    {
        get
        {
            if (number > 15)
                return CardWeight.wMax;
            return sortOrder[number];
        }
    }

    private readonly CardWeight[] sortOrder = new CardWeight[16] {
        CardWeight.wMin,
        CardWeight.wA,
        CardWeight.w2,
        CardWeight.w3,
        CardWeight.w4,
        CardWeight.w5,
        CardWeight.w6,
        CardWeight.w7,
        CardWeight.w8,
        CardWeight.w9,
        CardWeight.w10,
        CardWeight.wJ,
        CardWeight.wQ,
        CardWeight.wK,
        CardWeight.wJokerBlack,
        CardWeight.wJokerRed,
    };

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
        return number == other.number ? type.CompareTo(other.type) : weight.CompareTo(other.weight);
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
