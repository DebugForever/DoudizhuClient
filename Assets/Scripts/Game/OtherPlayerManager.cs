﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码

public class OtherPlayerManager : PlayerManagerBase
{
    [SerializeField] private float AiDelay = 1.0f;
    private new UIOtherPlayer view; //覆盖基类的这个，因为类型不同，这是抽象的必要代价

    protected override void Awake()
    {
        view = GetComponentInChildren<UIOtherPlayer>();
        base.view = view;
        gameManager = GetComponentInParent<GameManagerOffline>();
    }


    public override void StartTurn()
    {
        base.StartTurn();
        StartCoroutine(WaitPlayCard());
    }

    private IEnumerator WaitPlayCard()
    {
        if (IsAI)
        {
            yield return new WaitForSeconds(AiDelay);
            CardSet result = ai.PlayCard(CardManager.LastHand);
            Debug.LogFormat("player{0} CardSet:{1}", playerIndex, result);
            if (result.Type == CardSetType.Invalid || result.Type == CardSetType.None)
            {
                PassTurn();
                yield break;
            }
            cardHand.RemoveCards(result.Cards);
            view.RemoveCards(result.Cards);
            EventCenter.BroadCast(EventType.PlayerPlayCard, result);
            EndTurnPlayCard();
        }
        else
        {
            //玩家控制，这里没有，直接不出。
            //或者等待回合结束也行
            yield return new WaitForSeconds(AiDelay);
            PassTurn();
        }

    }
}
