using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol.Dto;
using ServerProtocol.SharedCode;

//view指视图，就是MVC中的View层
public class GameView : MonoBehaviour
{
    public UIMainPlayer mainPlayerView { get; private set; }
    public UIOtherPlayer player2View { get; private set; }
    public UIOtherPlayer player3View { get; private set; }
    public UIResultPanel resultPanel { get; private set; }
    public UIUnder3Cards under3Cards { get; private set; }


    private void Awake()
    {
        mainPlayerView = transform.Find("MainPlayer").GetComponent<UIMainPlayer>();
        player2View = transform.Find("Player2").GetComponent<UIOtherPlayer>();
        player3View = transform.Find("Player3").GetComponent<UIOtherPlayer>();
        resultPanel = transform.Find("ResultPanel").GetComponent<UIResultPanel>();
        under3Cards = transform.Find("Under3Cards").GetComponent<UIUnder3Cards>();

        EventCenter.AddListener<Card[]>(EventType.MainPlayerAddCards, MainPlayerAddCards);
        EventCenter.AddListener<Card[]>(EventType.Player2AddCards, Player2AddCards);
        EventCenter.AddListener<Card[]>(EventType.Player3AddCards, Player3AddCards);
        EventCenter.AddListener<Card[]>(EventType.MainPlayerRemoveCards, MainPlayerRemoveCards);
        EventCenter.AddListener<Card[]>(EventType.Player2RemoveCards, Player2RemoveCards);
        EventCenter.AddListener<Card[]>(EventType.Player3RemoveCards, Player3RemoveCards);
        EventCenter.AddListener(EventType.RefreshPlayerUI, RefreshPlayerUI);
        EventCenter.AddListener(EventType.MatchStart, MatchStart);
    }



    private void Start()
    {
        Init();
        resultPanel.HideNoAnim();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<Card[]>(EventType.MainPlayerAddCards, MainPlayerAddCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player2AddCards, Player2AddCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player3AddCards, Player3AddCards);
        EventCenter.RemoveListener<Card[]>(EventType.MainPlayerRemoveCards, MainPlayerRemoveCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player2RemoveCards, Player2RemoveCards);
        EventCenter.RemoveListener<Card[]>(EventType.Player3RemoveCards, Player3RemoveCards);
        EventCenter.RemoveListener(EventType.RefreshPlayerUI, RefreshPlayerUI);
        EventCenter.RemoveListener(EventType.MatchStart, MatchStart);
    }

    private void Init()
    {
        mainPlayerView.ClearCards();
        player2View.ClearCards();
        player3View.ClearCards();
    }

    /// <summary>
    /// 根据models里面的数据改变UI
    /// improve 冗余的操作多，即使只是改变准备状态都会使所有玩家UI重置一次，而不是精确的改变准备UI。
    /// 但是这样做的优点是可以少写很多方法。
    /// 需求很像Vue的ViewModel，即改变model会以最小的代价改变view
    /// </summary>
    private void RefreshPlayerUI()
    {
        var userList = Models.gameModel.roomModel.roomUserList;
        for (int i = 0; i < userList.Count; i++)
        {
            MatchRoomUserInfoDto user = userList[i];
            ResetPlayerUiByIndex(i, user, true);
        }
        for (int i = userList.Count; i < 3; i++) // 对于房间未满的情况，隐藏剩下位置的用户信息
        {
            ResetPlayerUiByIndex(i, null, false);
        }

    }

    public UIPlayerBase GetPlayerUiByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return mainPlayerView;
            case 1:
                return player2View;
            case 2:
                return player3View;
            default:
                return null;
        };
    }

    private void ResetPlayerUiByIndex(int index, MatchRoomUserInfoDto user, bool show)
    {
        switch (index)
        {
            case 0:
                if (show)
                    mainPlayerView.ResetPlayerUI(user);
                break;
            case 1:
                if (show)
                {
                    player2View.ShowInfo();
                    player2View.ResetPlayerUI(user);
                }
                else
                {
                    player2View.HideInfo();
                    player2View.HideStatusText(); // 退出房间时，准备提示也要隐藏
                }
                break;
            case 2:
                if (show)
                {
                    player3View.ShowInfo();
                    player3View.ResetPlayerUI(user);
                }
                else
                {
                    player3View.HideInfo();
                    player3View.HideStatusText();
                }
                break;
        }
    }

    /// <summary>
    /// 一局游戏开始，调整一些UI
    /// </summary>
    private void MatchStart()
    {
        mainPlayerView.HideStatusText();
        mainPlayerView.ButtonsSwitchGrabLandlord();
        mainPlayerView.HideButtons();
        player2View.HideStatusText();
        player3View.HideStatusText();
        under3Cards.ShowNoAnim();
    }

    /// <summary>
    /// 重开一局游戏
    /// </summary>
    public void MatchReset()
    {
        mainPlayerView.MatchReset();
        player2View.MatchReset();
        player3View.MatchReset();
        under3Cards.MatchReset();
    }

    private void MainPlayerAddCards(Card[] cards)
    {
        mainPlayerView.AddCards(cards);
    }

    private void Player2AddCards(Card[] cards)
    {
        player2View.AddCards(cards);
    }

    private void Player3AddCards(Card[] cards)
    {
        player3View.AddCards(cards);
    }

    private void MainPlayerRemoveCards(Card[] cards)
    {
        mainPlayerView.RemoveCards(cards);
    }


    private void Player2RemoveCards(Card[] cards)
    {
        player2View.RemoveCards(cards);
    }

    private void Player3RemoveCards(Card[] cards)
    {
        player3View.RemoveCards(cards);
    }


}
