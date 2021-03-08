using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol.SharedCode;

/// <summary>
/// 电脑玩家类
/// </summary>
public class AIPlayer
{
    private CardHand cardHand;

    public AIPlayer(CardHand cardHand)
    {
        this.cardHand = cardHand;
    }

    /// <summary>
    /// 电脑玩家出牌
    /// </summary>
    /// <param name="prevCardSet">上一手牌</param>
    /// <returns>出牌结果</returns>
    public CardSet PlayCard(CardSet prevCardSet)
    {
        //直接用提示功能
        //不会写AI，所以直接打能打过的较小的牌
        return cardHand.GetCardSetGreater(prevCardSet);
    }

    public bool GrabLandlord()
    {
        return cardHand.GetCardHandScore() >= 7;
    }
}
