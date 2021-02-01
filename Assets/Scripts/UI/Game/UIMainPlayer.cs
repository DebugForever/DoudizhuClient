using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPlayer : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text ratioText;
    private UnityEngine.UI.Text coinText;
    private UnityEngine.UI.Text usernameText;
    // === auto generated code end === 
    private Transform cardsTransform;
    private Image headIconImage;
    private List<MainPlayerCard> cards = new List<MainPlayerCard>();

    private int _coin;
    private int _ratio;



    private void Awake()
    {
        // === auto generated code begin === 
        usernameText = transform.Find("UsernamePanel/UsernameText").GetComponent<UnityEngine.UI.Text>();
        ratioText = transform.Find("InfoPanel/RatioPanel/Text").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("InfoPanel/CoinPanel/Text").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        cardsTransform = transform.Find("Cards");
        headIconImage = transform.Find("HeadIcon/HeadIconMask/HeadIconImage").GetComponent<Image>();
    }

    public string username
    {
        get => usernameText.text; set => usernameText.text = value;
    }

    public int coin
    {
        get => _coin; set { _coin = value; coinText.text = _coin.ToString(); }
    }

    public int ratio
    {
        get => _ratio; set { _ratio = value; ratioText.text = _ratio.ToString(); }
    }

    public Sprite headIcon
    {
        get => headIconImage.sprite;
        set => headIconImage.sprite = value;
    }

    public int[] GetSelectedCardsId()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            MainPlayerCard card = cards[i];
            if (card.selected)
                result.Add(i);
        }
        return result.ToArray();
    }

    public void RemoveSelectedCards()
    {
        int[] removeIndexs = GetSelectedCardsId();
        for (int i = 0; i < removeIndexs.Length; i++)
        {
            cards.RemoveAt(removeIndexs[i] - i);//删除会导致元素位置改变，所以要调整index
            Destroy(cardsTransform.GetChild(removeIndexs[i]).gameObject);//Destroy不会立即执行，所以不用调整index
        }

    }


    public void AddCard(Card card)
    {
        GameObject go = Instantiate(ResourceManager.GetMainPlayerCardPrefab());
        go.transform.SetParent(cardsTransform);
        MainPlayerCard mainPlayerCard = go.GetComponent<MainPlayerCard>();
        mainPlayerCard.card = card;
        cards.Add(mainPlayerCard);
    }

    public void AddCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            AddCard(card);
        }
    }

    public void ClearCards()
    {
        MyTools.DestroyAllChild(cardsTransform);
    }
}
