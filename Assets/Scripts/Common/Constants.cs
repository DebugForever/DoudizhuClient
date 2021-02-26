using System;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    /// <summary>场景名</summary>
    public static class SceneName
    {
        public const string Lobby = "2_Main";
    }

    public static class Game
    {
        /// <summary>每波发牌间隔</summary>
        public const float DealCardPeriod = 0.2f;
        /// <summary>一波发牌张数</summary>
        public const int DealCardTakeCount = 4;
    }
}

