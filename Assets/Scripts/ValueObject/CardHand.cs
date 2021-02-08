using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 用于管理和简化手牌信息的类
/// </summary>
public class CardHand
{
    #region 成员变量与访问器
    private List<Card> _cards;
    private List<Card> cards { get => _cards; set { _cards = value; cacheVaild = false; } }

    /// <summary>
    /// 获取全部手牌
    /// </summary>
    public Card[] GetCards() => cards.ToArray();

    public Dictionary<int, int> weightCountDict
    {
        get
        {
            if (!cacheVaild)
            {
                _weightCountDict = GetWeightCountDict();
                cacheVaild = true;
            }
            return _weightCountDict;
        }
    }
    private Dictionary<int, int> _weightCountDict = new Dictionary<int, int>();
    private bool cacheVaild; //避免每次重新计算_weightCountDict

    private Dictionary<int, int> GetWeightCountDict()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        foreach (Card card in cards)
        {
            if (dict.ContainsKey((int)card.weight))
                dict[(int)card.weight] += 1;
            else
                dict.Add((int)card.weight, 1);
        }

        return dict;
    }
    #endregion
    #region 作为容器操作
    public CardHand(Card[] cards)
    {
        this.cards = new List<Card>(cards);
        ResetHandIds();
    }

    public CardHand()
    {
        this.cards = new List<Card>();
    }

    public void AddCards(Card[] cards)
    {
        this.cards.AddRange(cards);
        ResetHandIds();
    }

    public void RemoveCards(Card[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            this.cards.RemoveAt(cards[i].handId - i);
        }
    }

    public void RemoveCards(int[] cardIndexs)
    {
        for (int i = 0; i < cardIndexs.Length; i++)
        {
            this.cards.RemoveAt(cardIndexs[i] - i);
        }
    }

    public void Clear()
    {
        this.cards.Clear();
    }

    /// <summary>
    /// 重新设置每张卡的HandId，手牌张数不会超过20，
    /// 故没有对删除添加做优化，直接重置即可
    /// </summary>
    private void ResetHandIds()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].handId = i;
        }
    }

    /// <summary>
    /// 以打牌最常见的方式排序，即降序排列
    /// </summary>
    public void Sort()
    {
        cards.Sort((a, b) => -a.CompareTo(b));
        ResetHandIds();
    }

    public void Sort(IComparer<Card> comparer)
    {
        cards.Sort(comparer);
        ResetHandIds();
    }


    #endregion
    #region 获取牌型
    /// <summary>
    /// 手牌中是否存在一张牌
    /// </summary>
    /// <param name="weight">牌的权值</param>
    /// <param name="minCount">最少需要几张</param>
    /// <returns></returns>
    public bool ExistCard(int weight, int minCount = 1)
    {
        if (weightCountDict.TryGetValue(weight, out int count))
            return count >= minCount;
        else
            return false;
    }

    /// <summary>
    /// 手牌中是否存在一个顺子
    /// </summary>
    /// <param name="weight">顺子最大牌的权值</param>
    /// <param name="minCount">每张牌最少需要几张</param>
    /// <returns></returns>
    public bool ExistStraight(int weight, int length, int minCount = 1)
    {
        for (int i = weight; i > weight - length; i--)
        {
            if (i < (int)CardWeight.w3 || i > (int)CardWeight.wA)//顺子只能包含3~A
                return false;
            if (!ExistCard(i, minCount))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 从手牌中获取指定卡牌count张，无验证
    /// </summary>
    private Card[] GetCards(int weight, int count = 1)
    {
        List<Card> result = new List<Card>();
        int nowCount = 0;
        foreach (Card card in cards)
        {
            if (nowCount >= count)
                break;
            if ((int)card.weight == weight)
            {
                result.Add(card);
                nowCount += 1;
            }
        }
        return result.ToArray();
    }

    /// <summary>
    /// 尝试从手牌中获取指定卡牌count张
    /// </summary>
    /// <param name="weight">指定的大小</param>
    /// <param name="count">至少多少张</param>
    /// <returns></returns>
    public bool TryGetCards(out Card[] cards, int weight, int count = 1)
    {
        cards = null;
        if (weightCountDict.TryGetValue(weight, out int hasCount))
        {
            if (hasCount >= count)
            {
                cards = GetCards(weight, count);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 尝试获取一个精确的顺子，支持非单顺
    /// </summary>
    /// <param name="weight">顺子的keyNumber（最大的一张）</param>
    /// <param name="length">顺子长度</param>
    /// <param name="straightRepeat">顺子重复数，比如双顺是2</param>
    /// <returns>是否获取成功</returns>
    public bool TryGetStraight(out Card[] cards, int weight, int length, int straightRepeat = 1)
    {
        cards = null;
        List<Card> result = new List<Card>();
        for (int i = weight; i > weight - length; i--)
        {
            if (i < (int)CardWeight.w3 || i > (int)CardWeight.wA)//顺子只能包含3~A
                return false;
            bool exist = TryGetCards(out Card[] arr, i, straightRepeat);
            if (!exist)
                return false;
            result.AddRange(arr);
        }

        cards = result.ToArray();
        return true;
    }

    /// <summary>
    /// 尝试获取一个精确的带其他卡牌的顺子，支持非单顺，不检查顺子长度
    /// </summary>
    /// <param name="weight">顺子的keyNumber（最大的一张）</param>
    /// <param name="length">顺子长度</param>
    /// <param name="straightRepeat">顺子重复数，比如双顺是2</param>
    /// <returns>是否获取成功</returns>
    /// <remarks>可以用来获取三带一等牌型，这些牌型可以看成长度为1的带牌顺子</remarks>
    private bool TryGetStraightWithSubCards(out Card[] cards, int weight, int length, int[] subCardCnts, int straightRepeat = 1)
    {
        if (subCardCnts.Length > 1 && length > 1)
            throw new NotSupportedException("不支持带多个的多顺");

        cards = null;
        List<Card> result = new List<Card>();

        //数字比较少，就不用树形数据结构了，
        //不写成(w>weight||w<weight-length)是因为用contains可读性好
        List<int> selectedWeights = new List<int>();

        //顺子部分，这里和上面的一样
        for (int i = weight; i > weight - length; i--)
        {
            if (i < (int)CardWeight.w3 || i > (int)CardWeight.wA)//顺子只能包含3~A
                return false;

            bool exist = TryGetCards(out Card[] arr, i, straightRepeat);
            selectedWeights.Add(i);

            if (!exist)
                return false;
            result.AddRange(arr);
        }

        //处理被带的卡牌，这些卡牌没有大小要求
        Array.Sort(subCardCnts);
        List<(int cnt, int weight)> cntWeightList = new List<(int cnt, int weight)>();
        foreach (var item in weightCountDict)
        {
            cntWeightList.Add((cnt: item.Value, weight: item.Key));
        }
        //cnt升序，然后weight升序排，这样得出的结果是较优的（优先带少的小牌）
        cntWeightList.Sort();

        //都是排好序的，所以直接扫一遍就可以了
        int index = 0;
        foreach (int requiredCnt in subCardCnts)
        {
            bool ok = false;
            for (; index < cntWeightList.Count; index++)
            {
                var item = cntWeightList[index];
                if (item.cnt > requiredCnt && !selectedWeights.Contains(item.weight))//满足需求并且没有拿过，可以拿这种卡牌
                {
                    //这一句实际上不需要，因为顺序扫，并且接下来不会用到selectedWeights
                    //但是加上保证加代码不出错
                    selectedWeights.Add(item.weight);
                    result.AddRange(GetCards(item.weight, requiredCnt));
                    index++;
                    ok = true;
                    break;
                }
            }

            //没有全部匹配上，ok用于处理边界条件，即最后一个和最后一个刚好匹配不能return false
            if (index >= cntWeightList.Count && !ok)
                return false;
        }

        cards = result.ToArray();
        return true;
    }

    /// <summary>
    /// 尝试获取一个精确的牌型（精确到第一关键字）
    /// </summary>
    /// <returns>是否获取成功</returns>
    private bool TryGetExactCardSet(out CardSet cardSet, CardSetType setType, int keyNumber, int repeat)
    {
        cardSet = null;
        Card[] cardsResult = null;
        switch (setType)
        {
            case CardSetType.Invalid:
                cardSet = CardSet.Invalid;
                return true;
            case CardSetType.None:
                cardSet = CardSet.None;
                return true;
            case CardSetType.JokerBomb:
                if (ExistCard((int)CardWeight.wJokerBlack) && ExistCard((int)CardWeight.wJokerRed))
                {
                    cardSet = CardSet.JokerBomb;
                    return true;
                }
                return false;
            case CardSetType.Single:
            case CardSetType.Pair:
            case CardSetType.Triple:
            case CardSetType.Bomb:
                if (!TryGetExactCardSetPart1(out cardsResult))
                {
                    return false;
                }
                break;
            case CardSetType.Straight:
            case CardSetType.DoubleStraight:
            case CardSetType.TripleStraight:
                if (!TryGetExactCardSetPart2(out cardsResult))
                {
                    return false;
                }
                break;
            case CardSetType.TripleWithOne:
            case CardSetType.TripleWithPair:
            case CardSetType.TripleStraightWithOne:
            case CardSetType.TripleStraightWithPair:
            case CardSetType.QuadraWithTwo:
            case CardSetType.QuadraWithTwoPairs:
                if (!TryGetExactCardSetPart3(out cardsResult))
                {
                    return false;
                }
                break;
            default:
                break;
        }
        cardSet = new CardSet(setType, keyNumber, repeat, cardsResult);
        return true;

        bool TryGetExactCardSetPart1(out Card[] cardsResult1)//加1避免名称冲突（vs可以通过编译，unity不行）
        {
            int repeatCount = 1;
            switch (setType)
            {
                case CardSetType.Single:
                    repeatCount = 1;
                    break;
                case CardSetType.Pair:
                    repeatCount = 2;
                    break;
                case CardSetType.Triple:
                    repeatCount = 3;
                    break;
                case CardSetType.Bomb:
                    repeatCount = 4;
                    break;
            }

            return TryGetCards(out cardsResult1, keyNumber, repeatCount);
        }

        bool TryGetExactCardSetPart2(out Card[] cardsResult1)
        {
            int straightRepeat = 1;//这是几顺，是单顺，双顺还是三顺
            switch (setType)
            {
                case CardSetType.Straight:
                    straightRepeat = 1;
                    break;
                case CardSetType.DoubleStraight:
                    straightRepeat = 2;
                    break;
                case CardSetType.TripleStraight:
                    straightRepeat = 3;
                    break;
            }
            return TryGetStraight(out cardsResult1, keyNumber, repeat, straightRepeat);
        }

        bool TryGetExactCardSetPart3(out Card[] cardsResult1)
        {
            int length = 1;
            int straightRepeat = 1;
            int[] subCardCnts = null;
            switch (setType)
            {
                case CardSetType.TripleWithOne:
                    length = 1;
                    straightRepeat = 3;
                    subCardCnts = new int[] { 1 };
                    break;
                case CardSetType.TripleWithPair:
                    length = 1;
                    straightRepeat = 3;
                    subCardCnts = new int[] { 2 };
                    break;
                case CardSetType.TripleStraightWithOne:
                    length = repeat;
                    straightRepeat = 3;
                    subCardCnts = new int[] { 1 };
                    break;
                case CardSetType.TripleStraightWithPair:
                    length = repeat;
                    straightRepeat = 3;
                    subCardCnts = new int[] { 2 };
                    break;
                case CardSetType.QuadraWithTwo:
                    length = 1;
                    straightRepeat = 4;
                    subCardCnts = new int[] { 1, 1 };
                    break;
                case CardSetType.QuadraWithTwoPairs:
                    length = 1;
                    straightRepeat = 4;
                    subCardCnts = new int[] { 2, 2 };
                    break;
            }
            return TryGetStraightWithSubCards(out cardsResult1, keyNumber, length, subCardCnts, straightRepeat);
        }
    }

    /// <summary>
    /// 尝试获取一个可以压住指定牌型的相同牌型
    /// </summary>
    /// <returns>是否成功</returns>
    private bool TryGetCardSetSameTypeGreater(out CardSet result, CardSet prevSet)
    {
        result = null;
        for (int key = prevSet.KeyNumber + 1; key < (int)CardWeight.wMax; key++)
        {
            if (TryGetExactCardSet(out result, prevSet.SetType, key, prevSet.RepeatCount))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 尝试从手牌中获取一个点数至少为minWeight的炸弹
    /// </summary>
    /// <returns>是否成功</returns>
    private bool TryGetCardSetBomb(out CardSet result, int minWeight = (int)CardWeight.w3)
    {
        result = null;
        for (int key = minWeight; key < (int)CardWeight.wMax; key++)
        {
            if (TryGetExactCardSet(out result, CardSetType.Bomb, key, 1))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取一个可以压住指定牌型的牌型，如果没有则返回牌型Invalid
    /// 这个牌型为较优策略（不会整最优策略）
    /// </summary>
    /// <param name="prevSet"></param>
    /// <returns></returns>
    public CardSet GetCardSetGreater(CardSet prevSet)
    {
        CardSet result;
        if (prevSet.SetType == CardSetType.JokerBomb) //没有比王炸大的牌
            return CardSet.Invalid;
        else if (prevSet.SetType == CardSetType.Bomb) //只有炸弹和王炸比炸弹大
        {
            if (TryGetCardSetBomb(out result, prevSet.KeyNumber + 1))
                return result;

            if (TryGetExactCardSet(out result, CardSetType.JokerBomb, 0, 0))//王炸不需要后两个参数
                return result;
        }
        else //普通牌型，从小往大判断
        {
            if (TryGetCardSetSameTypeGreater(out result, prevSet))
                return result;

            if (TryGetCardSetBomb(out result))
                return result;

            if (TryGetExactCardSet(out result, CardSetType.JokerBomb, 0, 0))//王炸不需要后两个参数
                return result;
        }

        return CardSet.Invalid;
    }

    #endregion
}
