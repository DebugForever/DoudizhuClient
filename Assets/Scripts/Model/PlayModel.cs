using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol.Dto;
using ServerProtocol.SharedCode;

namespace MyModel
{
    public class PlayModel
    {
        public CardSet lastHand;
        public CardHand MainPlayerHand;
        public int passCount;

        public PlayModel()
        {
            lastHand = CardSet.None;
            MainPlayerHand = null;
            passCount = 0;
        }
    }
}
