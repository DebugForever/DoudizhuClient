﻿public enum EventType
{
    ///<summary>用于暂时测试一个功能，一般不使用</summary>
    TestEvent,
    ///<summary>显示注册面板</summary>
    UIShowRegister,
    ///<summary>显示登陆面板</summary>
    UIShowLogin,
    ///<summary>显示指定的提示文字</summary>
    UIFlashHint,
    ///<summary>显示排行榜面板</summary>
    UIShowRankList,
    ///<summary>设置排行榜信息</summary>
    UISetRankList,
    ///<summary>出牌按钮</summary>
    PlayCard,
    ///<summary>不出按钮</summary>
    PassTurn,
    ///<summary>提示按钮</summary>
    PlayCardHint,
    ///<summary>抢地主按钮</summary>
    GrabLandlord,
    ///<summary>不抢按钮</summary>
    NoGrabLandlord,
    ///<summary>准备按钮</summary>
    Ready,
    ///<summary>取消准备按钮</summary>
    UnReady,
    ///<summary>给主玩家添加一些牌</summary>
    MainPlayerAddCards,
    ///<summary>给玩家2添加一些牌</summary>
    Player2AddCards,
    ///<summary>给玩家3添加一些牌</summary>
    Player3AddCards,
    ///<summary>删除主玩家一些牌</summary>
    MainPlayerRemoveCards,
    ///<summary>删除玩家2一些牌</summary>
    Player2RemoveCards,
    ///<summary>删除玩家3一些牌</summary>
    Player3RemoveCards,
    ///<summary>设置底牌</summary>
    SetUnder3Cards,
    ///<summary>根据RoomModel更改玩家UI</summary>
    RefreshPlayerUI,
    ///<summary>任意玩家出牌</summary>
    PlayerPlayCard,
    ///<summary>玩家出牌超时</summary>
    PlayerTimeUp,
    ///<summary>重开按钮</summary>
    MatchReset,
    ///<summary>退出按钮</summary>
    MatchExit,
    ///<summary>一局游戏结束</summary>
    MatchOver,
    ///<summary>发牌结束</summary>
    DealCardOver,
    ///<summary>自己进入匹配房间</summary>
    SelfEnterRoom,
}