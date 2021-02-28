using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ResourceManager
{
    private static Dictionary<string, Sprite> SpriteDict = new Dictionary<string, Sprite>();//缓存用
    //因为一局游戏每一张卡都会用上，所以直接读取所有卡牌，数组下标:[类型][数字]
    private static Sprite[][] cards = new Sprite[6][]{
    new Sprite[14],
    new Sprite[14],
    new Sprite[14],
    new Sprite[14],
    new Sprite[1],
    new Sprite[1],
    };
    private static Sprite cardBack;

    public static Sprite GetHeadIcon(string iconName)
    {
        if (SpriteDict.ContainsKey(iconName))
        {
            return SpriteDict[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("HeadIcon/headIcon");//当前的头像是图集，所以用loadAll
            int index = int.Parse(iconName.Split('_')[1]);//iconName的命名格式：HeadIcon_i，i是数字
            SpriteDict.Add(iconName, sprites[index]);
            return sprites[index];
        }
    }

    public static GameObject GetLoadingPanel()
    {
        return Resources.Load<GameObject>("Prefabs/LoadingPanel");
    }

    public static GameObject GetRankListItem()
    {
        return Resources.Load<GameObject>("Prefabs/RankListItem");
    }

    private static void GetAllCards()
    {
        //载入桃心梅方
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                cards[i][j] = Resources.Load<Sprite>($"Cards/card_{i}_{j}");
            }
        }

        //大小王
        Sprite[] jokers = Resources.LoadAll<Sprite>("Cards/joker");
        cards[4][0] = jokers[0];
        cards[5][0] = jokers[1];
    }

    public static Sprite GetCard(CardType cardType, int number)
    {
        //懒加载
        if (cards[0][1] == null)
            GetAllCards();

        if (cardType == CardType.JokerBlack)
            return cards[4][0];
        else if (cardType == CardType.JokerRed)
            return cards[5][0];
        else
        {
            if (number < 1 || number > 13)
                Debug.LogWarningFormat("number {0} is not vaild!", number);
            return cards[(int)cardType][number];
        }
    }

    public static Sprite GetCardBack()
    {
        if (!cardBack)
        {
            cardBack = Resources.Load<Sprite>("Cards/card_back");
        }
        return cardBack;
    }

    public static GameObject GetSingleCardPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/SingleCard");
    }

    public static GameObject GetMainPlayerCardPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/MainPlayerCard");
    }
}