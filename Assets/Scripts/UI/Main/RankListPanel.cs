using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ServerProtocol.Dto;

public class RankListPanel : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button closeButton;
    private Transform scrollViewContentTransform;
    // === auto generated code end === 

    private Vector3 origScale;

    void Awake()
    {
        // === auto generated code begin === 
        closeButton = transform.Find("CloseButton").GetComponent<UnityEngine.UI.Button>();
        scrollViewContentTransform = transform.Find("ScrollView/Viewport/Content");
        // === auto generated code end === 
        closeButton.onClick.AddListener(Hide);

        EventCenter.AddListener(EventType.UIShowRankList, Show);
        EventCenter.AddListener<RankListDto>(EventType.UISetRankList, GenerateRankList);

        //初始处于不显示状态
        origScale = transform.localScale;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.UIShowRankList, Show);
        EventCenter.RemoveListener<RankListDto>(EventType.UISetRankList, GenerateRankList);
    }

    void GenerateRankList(RankListDto dto)
    {
        //先清空排行榜里面的东西，再重新生成一份
        foreach (Transform child in scrollViewContentTransform)
        {
            Destroy(child.gameObject);
        }

        GameObject rankListItemPrefab = ResourceManager.GetRankListItem();
        List<RankItemDto> list = dto.list;
        for (int i = 0; i < list.Count; i++)
        {
            RankItemDto item = list[i];
            GameObject rankListItem = Instantiate(rankListItemPrefab);
            rankListItem.GetComponent<RankListItem>().SetInfo(i + 1, item.username, item.coin);
            rankListItem.transform.SetParent(scrollViewContentTransform);
            rankListItem.transform.localScale = Vector3.one;
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
        transform.DOScale(origScale, 0.3f);
    }

    void Hide()
    {
        transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
    }

}