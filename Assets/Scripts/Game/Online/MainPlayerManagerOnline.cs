using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol.SharedCode;

public class MainPlayerManagerOnline : MonoBehaviour
{
    private UIMainPlayer view;


    private void Awake()
    {
        view = GetComponentInChildren<UIMainPlayer>();

        EventCenter.AddListener(EventType.Ready, Ready);
        EventCenter.AddListener(EventType.UnReady, UnReady);
        EventCenter.AddListener(EventType.GrabLandlord, GrabLandlord);
        EventCenter.AddListener(EventType.NoGrabLandlord, NoGrabLandlord);
        EventCenter.AddListener(EventType.PlayCard, OnPlayCardClicked);
        EventCenter.AddListener(EventType.PassTurn, OnPassTurnClicked);
        EventCenter.AddListener(EventType.PlayCardHint, OnPlayCardHintClicked);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.Ready, Ready);
        EventCenter.RemoveListener(EventType.UnReady, UnReady);
        EventCenter.RemoveListener(EventType.GrabLandlord, GrabLandlord);
        EventCenter.RemoveListener(EventType.NoGrabLandlord, NoGrabLandlord);
        EventCenter.RemoveListener(EventType.PlayCard, OnPlayCardClicked);
        EventCenter.RemoveListener(EventType.PassTurn, OnPassTurnClicked);
        EventCenter.RemoveListener(EventType.PlayCardHint, OnPlayCardHintClicked);
    }

    private void Ready()
    {
        view.SetStatusText("已准备");
        view.ShowStatusText();
        Models.gameModel.roomModel.Ready(Models.gameModel.userInfoDto);
        NetMsgCenter.Instance.SendReadyMsg(true);
    }

    private void UnReady()
    {
        view.HideStatusText();
        Models.gameModel.roomModel.UnReady(Models.gameModel.userInfoDto);
        NetMsgCenter.Instance.SendReadyMsg(false);
    }

    private void GrabLandlord()
    {
        NetMsgCenter.Instance.SendGrabLandlordMsg(true);
    }

    private void NoGrabLandlord()
    {
        NetMsgCenter.Instance.SendGrabLandlordMsg(false);
    }

    public void StartTurn()
    {
        view.ShowButtons();
    }

    public void EndTurn()
    {
        view.HideButtons();
    }

    private void OnPlayCardClicked()
    {
        Card[] cards = view.GetSelectedCards();
        CardSet cardSet = CardSet.GetCardSet(cards);

        if (cards.Length == 0)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "请选择要出的牌");
            return;
        }

        if (cardSet.Type == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无效的组合");
            return;
        }

        CardSet lastHand = Models.gameModel.playModel.lastHand;
        if (lastHand != null && !cardSet.IsGreaterThan(lastHand))
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "无法大过上家");
            return;
        }

        //出牌合法，向服务器发送数据
        NetMsgCenter.Instance.SendPlayCardMsg(cardSet);
    }

    private void OnPassTurnClicked()
    {
        NetMsgCenter.Instance.SendPassTurnMsg();
    }

    private void OnPlayCardHintClicked()
    {
        var cardHand = Models.gameModel.playModel.MainPlayerHand;
        CardSet hintSet = cardHand.GetCardSetGreater(Models.gameModel.playModel.lastHand);
        if (hintSet.Type == CardSetType.Invalid)
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "没有可出的牌");
            NetMsgCenter.Instance.SendPassTurnMsg();
            return;
        }
        view.UnselectAllCard();
        view.SelectCards(hintSet.Cards);
    }
}
