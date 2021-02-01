public enum EventType
{
    TestEvent,//用于暂时测试一个功能，一般不使用
    UIShowRegister,//显示注册面板
    UIShowLogin,//显示登陆面板
    UIFlashHint,//显示指定的提示文字
    UIShowRankList,//显示排行榜面板
    UISetRankList,//设置排行榜信息
    PlayCard,//出牌
    PassTurn,//不出
    PlayCardHint,//提示
    MainPlayerAddCards,//给主玩家添加一些牌
    Player2AddCards,//给玩家2添加一些牌
    Player3AddCards,//给玩家3添加一些牌
    MainPlayerRemoveCards,//删除主玩家一些牌
    Player2RemoveCards,//删除玩家2一些牌
    Player3RemoveCards,//删除玩家3一些牌
}