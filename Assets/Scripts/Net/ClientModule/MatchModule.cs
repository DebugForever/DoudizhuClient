using ServerProtocol.Dto;
using ServerProtocol.Code;
using MyModel;
using System;
using ServerProtocol.SharedCode;

public class MatchModule : ClientModule
{
    public override void OnReceiveNetMsg(int subOpCode, object value)
    {

        switch (subOpCode)
        {
            case MatchCode.EnterSRes:
                Models.gameModel.roomModel = new RoomModel((MatchRoomDto)value);
                EventCenter.BroadCast(EventType.SelfEnterRoom);
                EventCenter.BroadCast(EventType.RefreshPlayerUI);
                break;
            case MatchCode.EnterBrd:
                Models.gameModel.roomModel.EnterRoom((UserInfoDto)value);
                EventCenter.BroadCast(EventType.RefreshPlayerUI);
                break;
            case MatchCode.ExitSRes:
                //服务器暂时没有对退出房间请求返回消息
                break;
            case MatchCode.ExitBrd:
                Models.gameModel.roomModel.ExitRoom((UserInfoDto)value);
                EventCenter.BroadCast(EventType.RefreshPlayerUI);
                break;
            case MatchCode.ReadySRes:
                //服务器暂时没有对准备请求返回消息
                break;
            case MatchCode.ReadyBrd:
                Models.gameModel.roomModel.Ready((UserInfoDto)value);
                EventCenter.BroadCast(EventType.RefreshPlayerUI);
                break;
            case MatchCode.UnReadyBrd:
                Models.gameModel.roomModel.UnReady((UserInfoDto)value);
                EventCenter.BroadCast(EventType.RefreshPlayerUI);
                break;
            case MatchCode.GameStartBrd:
                Models.gameModel.playModel = new PlayModel();
                EventCenter.BroadCast(EventType.MatchStart);
                break;
            default:
                break;
        }
    }
}

