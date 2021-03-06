﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码
using ServerProtocol.SharedCode;
public class MainPlayerManagerOffline : PlayerManagerOfflineBase
{
    private new UIMainPlayer view;

    public void Init(CardHand cardHand)
    {
        this.cardHand = cardHand;
        this.playerIndex = 1;
    }

    protected override void Awake()
    {
        view = GetComponentInChildren<UIMainPlayer>();
        base.view = view;//base.view 是没有赋值的
        gameManager = GetComponentInParent<GameManagerOffline>();

        EventCenter.AddListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.AddListener(EventType.PassTurn, PassTurn);
        EventCenter.AddListener(EventType.PlayCardHint, HintCards);
        EventCenter.AddListener(EventType.GrabLandlord, GrabLandlord);
        EventCenter.AddListener(EventType.NoGrabLandlord, NoGrabLandlord);
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.PlayCard, MainPlayerPlayCard);
        EventCenter.RemoveListener(EventType.PassTurn, PassTurn);
        EventCenter.RemoveListener(EventType.PlayCardHint, HintCards);
        EventCenter.RemoveListener(EventType.GrabLandlord, GrabLandlord);
        EventCenter.RemoveListener(EventType.NoGrabLandlord, NoGrabLandlord);
    }

    protected override void EndTurnPlayCard()
    {
        base.EndTurnPlayCard();
        view.HideButtons();
    }

    protected override void PassTurn()
    {
        base.PassTurn();
        view.UnselectAllCard();
        view.HideButtons();
    }

    protected override void EndTurnGrabLandlord(bool isGrab)
    {
        base.EndTurnGrabLandlord(isGrab);
        view.HideButtons();
    }

    private void GrabLandlord()
    {
        EndTurnGrabLandlord(true);
    }

    private void NoGrabLandlord()
    {
        EndTurnGrabLandlord(false);
    }

    public override void StartTurn(TurnType turnType)
    {
        base.StartTurn(turnType);
        if (turnType == TurnType.GarbLandlord)
        {
            view.ButtonsSwitchGrabLandlord();
        }
        else if (turnType == TurnType.PlayCard)
        {
            view.ButtonsSwitchPlayCard();
        }
        view.ShowButtons();
        view.EnableButtons();
    }

    private void FixedUpdate()
    {
        //if (IsAI && isMyTurn)
        //{
        //    if (ai == null)
        //        ai = new AIPlayer(cardHand);
        //    CardSet cardSet = ai.PlayCard(CardManager.LastHand);
        //    PlayCard(cardSet);
        //}
    }

    /// <summary>
    /// 主玩家出牌
    /// </summary>
    private void MainPlayerPlayCard()
    {
        if (!isMyTurn)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "现在无法出牌");
            return;
        }

        Card[] cards = view.GetSelectedCards();
        if (cards.Length == 0)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "请选择要出的牌");
            return;
        }

        CardSet cardSet = CardSet.GetCardSet(cards);
        Debug.LogFormat("main player cardset:{0}", cardSet);
        if (cardSet.Type == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无效的组合");
            view.UnselectAllCard();
        }
        else if (!cardSet.IsGreaterThan(CardManager.LastHand))//牌型不同，或者没对方大
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无法大过上家");
            view.UnselectAllCard();
        }
        else //合法的出牌
        {
            PlayCard(cardSet);
        }
    }

    protected override void PlayCard(CardSet cardSet)
    {
        base.PlayCard(cardSet);
        view.UnselectAllCard();
    }

    private void HintCards()
    {
        if (!isMyTurn)
        {
            return;
        }
        CardSet hintSet = cardHand.GetCardSetGreater(CardManager.LastHand);
        if (hintSet.Type == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "没有可出的牌");
            PassTurn();
            return;
        }
        view.UnselectAllCard();
        view.SelectCards(hintSet.Cards);
    }

}
