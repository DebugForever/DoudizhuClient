using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示出来的一张卡牌
/// </summary>
public class SingleCard : MonoBehaviour
{
    protected Card _card;
    protected Image image;
    protected bool isBack = false;

    protected void Awake()
    {
        image = GetComponent<Image>();
    }

    public Card card
    {
        get => _card;
        set
        {
            _card = value;
            if (!isBack)//如果是背面，则不需要替换图片
                image.sprite = _card.GetSprite();
        }
    }

    public void FaceUp()
    {
        isBack = false;
        image.sprite = _card.GetSprite();
    }

    public void FaceDown()
    {
        isBack = true;
        image.sprite = ResourceManager.GetCardBack();
    }

    public void Flip()
    {
        if (isBack)
            FaceUp();
        else
            FaceDown();
    }
}
