using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionButtons : HideablePanel
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button noButton;
    private UnityEngine.UI.Button yesButton;
    private UnityEngine.UI.Button hintButton;
    private UnityEngine.UI.Button grabButton;
    private UnityEngine.UI.Button noGrabButton;
    private UnityEngine.UI.Button readyButton;
    private UnityEngine.UI.Text readyButtonText;
    // === auto generated code end === 

    /// <summary>准备按钮的状态，true表示现在是[准备]</summary>
    private bool readyBtnStatus = true;

    private void Awake()
    {
        // === auto generated code begin === 
        noButton = transform.Find("NoButton").GetComponent<UnityEngine.UI.Button>();
        yesButton = transform.Find("YesButton").GetComponent<UnityEngine.UI.Button>();
        hintButton = transform.Find("HintButton").GetComponent<UnityEngine.UI.Button>();
        grabButton = transform.Find("GrabButton").GetComponent<UnityEngine.UI.Button>();
        noGrabButton = transform.Find("NoGrabButton").GetComponent<UnityEngine.UI.Button>();
        readyButton = transform.Find("ReadyButton").GetComponent<UnityEngine.UI.Button>();
        readyButtonText = transform.Find("ReadyButton/Text").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 

        //监听事件
        noButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PassTurn); });
        yesButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCard); });
        hintButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCardHint); });
        grabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.GrabLandlord); });
        noGrabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.NoGrabLandlord); });
        readyButton.onClick.AddListener(OnReadyBtnClicked);

        EventCenter.AddListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    private void OnReadyBtnClicked()
    {
        if (readyBtnStatus)
        {
            readyButtonText.text = "取消准备";
            EventCenter.BroadCast(EventType.Ready);
        }
        else
        {
            readyButtonText.text = "准备";
            EventCenter.BroadCast(EventType.UnReady);
        }
        readyBtnStatus = !readyBtnStatus;
    }

    public void SwitchPlayCard()
    {
        noButton.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        hintButton.gameObject.SetActive(true);
        grabButton.gameObject.SetActive(false);
        noGrabButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
    }

    public void SwitchGrabLandlord()
    {
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);
        grabButton.gameObject.SetActive(true);
        noGrabButton.gameObject.SetActive(true);
        readyButton.gameObject.SetActive(false);
    }

    public void SwitchReady()
    {
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);
        grabButton.gameObject.SetActive(false);
        noGrabButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(true);
    }

    public void DisableButtons()
    {
        noButton.interactable = false;
        yesButton.interactable = false;
        hintButton.interactable = false;
        grabButton.interactable = false;
        noGrabButton.interactable = false;
        readyButton.interactable = false;
    }

    public void EnableButtons()
    {
        noButton.interactable = true;
        yesButton.interactable = true;
        hintButton.interactable = true;
        grabButton.interactable = true;
        noGrabButton.interactable = true;
        readyButton.interactable = true;
    }

    private void OnSelfEnterRoom()
    {
        ShowNoAnim();
        SwitchReady();
    }
}
