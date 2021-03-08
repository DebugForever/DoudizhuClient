using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol.SharedCode;
using ServerProtocol.Dto;
using ServerProtocol.Code;

public class GameManagerOnline : MonoBehaviour
{
    private GameView view;
    private MainPlayerManagerOnline p1Manager;

    void Awake()
    {
        view = GetComponentInChildren<GameView>();
        p1Manager = transform.Find("MainPlayer").GetComponent<MainPlayerManagerOnline>();

        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.DealCardBrd, HandleDealCardBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.GrabLandlordBrd, HandleGrabLandlordBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.NoGrabLandlordBrd, HandleNoGrabLandlordBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.StartTurnBrd, HandleStartTurnBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.EndTurnBrd, HandleEndTurnBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.GrabLandlordEndBrd, HandleGarbLandlordEndBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.PlayCardBrd, HandlePlayCardBrd);
        NetMsgCenter.Instance.ListenServerMsg(OpCode.play, PlayCode.PassPlayCardBrd, HandlePassPlayCardBrd);
    }

    private void Start()
    {
        view.player2View.HideInfo();
        view.player3View.HideInfo();
        view.under3Cards.HideNoAnim();

        //进入游戏之后直接匹配，不整什么按钮了
        NetMsgCenter.Instance.SendEnterRoomMsg();
    }

    private void OnDestroy()
    {
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.DealCardBrd, HandleDealCardBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.GrabLandlordBrd, HandleGrabLandlordBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.NoGrabLandlordBrd, HandleNoGrabLandlordBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.StartTurnBrd, HandleStartTurnBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.EndTurnBrd, HandleEndTurnBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.GrabLandlordEndBrd, HandleGarbLandlordEndBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.PlayCardBrd, HandlePlayCardBrd);
        NetMsgCenter.Instance.UnListenServerMsg(OpCode.play, PlayCode.PassPlayCardBrd, HandlePassPlayCardBrd);
    }

    private void HandleDealCardBrd(object obj)
    {
        DealCardDto dto = obj as DealCardDto;
        Models.gameModel.playModel.MainPlayerHand = new CardHand(dto.cards);
        view.mainPlayerView.ClearCards();
        view.player2View.ClearCards();
        view.player3View.ClearCards();
        view.mainPlayerView.AddCards(dto.cards);
        view.player2View.AddFaceDownCards(dto.cards.Length);
        view.player3View.AddFaceDownCards(dto.cards.Length);
    }

    private UIPlayerBase GetPlayerUiByServerIndex(object obj)
    {
        int serverPlayerIndex = (int)obj;
        int clientPlayerIndex = Models.gameModel.roomModel.ServerPlaceIndexToClient(serverPlayerIndex);
        return view.GetPlayerUiByIndex(clientPlayerIndex);
    }

    private void HandleGrabLandlordBrd(object obj)
    {
        UIPlayerBase playerView = GetPlayerUiByServerIndex(obj);
        playerView.SetStatusText("抢地主");
        playerView.ShowStatusText();
    }

    private void HandleNoGrabLandlordBrd(object obj)
    {
        UIPlayerBase playerView = GetPlayerUiByServerIndex(obj);
        playerView.SetStatusText("不抢");
        playerView.ShowStatusText();
    }

    private void HandleStartTurnBrd(object obj)
    {
        UIPlayerBase playerView = GetPlayerUiByServerIndex(obj);
        playerView.StartTimer();
        int serverPlayerIndex = (int)obj;
        int clientPlayerIndex = Models.gameModel.roomModel.ServerPlaceIndexToClient(serverPlayerIndex);
        if (clientPlayerIndex == 0)//是自己
        {
            p1Manager.StartTurn();
        }
    }

    private void HandleEndTurnBrd(object obj)
    {
        UIPlayerBase playerView = GetPlayerUiByServerIndex(obj);
        playerView.StopTimer();
        int serverPlayerIndex = (int)obj;
        int clientPlayerIndex = Models.gameModel.roomModel.ServerPlaceIndexToClient(serverPlayerIndex);
        if (clientPlayerIndex == 0)//是自己
        {
            p1Manager.EndTurn();
        }
    }

    private void HandleGarbLandlordEndBrd(object obj)
    {
        view.mainPlayerView.ButtonsSwitchPlayCard();
        var dto = obj as GrabLandlordEndDto;
        int clientLandlordIndex = Models.gameModel.roomModel.ServerPlaceIndexToClient(dto.landlordIndex);
        view.under3Cards.SetUnderCards(dto.UnderCards);
        view.under3Cards.CardsFaceUp();
        if (clientLandlordIndex == 0)
        {
            CardHand mainPlayerHand = Models.gameModel.playModel.MainPlayerHand;
            mainPlayerHand.AddCards(dto.UnderCards);
            mainPlayerHand.Sort();
            view.mainPlayerView.ClearCards();
            view.mainPlayerView.AddCards(mainPlayerHand.GetCards());
        }
        else
        {
            UIPlayerBase playerView = view.GetPlayerUiByIndex(clientLandlordIndex);
            playerView.AddFaceDownCards(dto.UnderCards.Length);
        }
        //view.under3Cards.Show();
    }

    private void HandlePlayCardBrd(object obj)
    {
        PlayCardDto playCardDto = obj as PlayCardDto;
        CardSet cardSet = playCardDto.cardSet;
        int clientPlayerIndex = Models.gameModel.roomModel.ServerPlaceIndexToClient(playCardDto.playerIndex);
        UIPlayerBase playerView = view.GetPlayerUiByIndex(clientPlayerIndex);
        playerView.lastHandCards.SetCards(cardSet.Cards);
        playerView.HideStatusText();
        Models.gameModel.playModel.lastHand = cardSet;
        if (clientPlayerIndex == 0)
        {
            //是自己的话，移除手牌里的卡
            playerView.RemoveCards(cardSet.Cards);
            Models.gameModel.playModel.MainPlayerHand.RemoveCards(cardSet.Cards);
        }
        else
        {
            //不是自己的话，直接减少卡牌数量
            playerView.RemoveRightCards(cardSet.Cards.Length);
        }
    }

    private void HandlePassPlayCardBrd(object obj)
    {
        UIPlayerBase playerView = GetPlayerUiByServerIndex(obj);
        playerView.lastHandCards.ClearCards();
        playerView.SetStatusText("不出");
        playerView.ShowStatusText();
        Models.gameModel.playModel.passCount += 1;
        if (Models.gameModel.playModel.passCount >= 2)
            Models.gameModel.playModel.lastHand = CardSet.None;
    }



}
