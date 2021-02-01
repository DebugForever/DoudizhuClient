using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 主玩家的卡牌，需要处理选择
/// </summary>
public class MainPlayerCard : SingleCard, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private bool changed = false;

    ///// <summary>
    ///// 标识自己是手牌里的第几张，用于回调（比如删除时指定目标）
    ///// 管理卡牌的类会使用，自己不用这个id
    ///// </summary>
    //public int id { get; set; }
    public bool selected { get; private set; }

    /// <summary>
    /// 切换选择状态，这里通过事件来实现，因为主玩家的手牌是唯一的
    /// 但是还是应该做成回调的形式以支持可以每个实例独立运行
    /// 或者可以直接在这里塞一个管理器的引用（像toggle group一样）
    /// </summary>
    private void ToggleSelected()
    {
        changed = true;
        selected = !selected;
        if (selected)
        {
            OnSelected();
        }
        else
        {
            OnUnselected();
        }

    }

    private void OnSelected()
    {
        image.color = Color.yellow;
    }


    private void OnUnselected()
    {
        image.color = Color.white;
    }




    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleSelected();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && !changed)
        {
            ToggleSelected();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            changed = false;
        }
    }


}
