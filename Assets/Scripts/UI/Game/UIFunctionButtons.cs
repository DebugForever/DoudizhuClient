using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionButtons : HideablePanel
{
    // === auto generated code begin === 
    protected UnityEngine.UI.Button noButton;
    protected UnityEngine.UI.Button yesButton;
    protected UnityEngine.UI.Button hintButton;
    protected UnityEngine.UI.Button grabButton;
    protected UnityEngine.UI.Button noGrabButton;
    // === auto generated code end === 


    protected virtual void Awake()
    {
        // === auto generated code begin === 
        noButton = transform.Find("NoButton").GetComponent<UnityEngine.UI.Button>();
        yesButton = transform.Find("YesButton").GetComponent<UnityEngine.UI.Button>();
        hintButton = transform.Find("HintButton").GetComponent<UnityEngine.UI.Button>();
        grabButton = transform.Find("GrabButton").GetComponent<UnityEngine.UI.Button>();
        noGrabButton = transform.Find("NoGrabButton").GetComponent<UnityEngine.UI.Button>();
        // === auto generated code end === 

        //监听事件
        noButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PassTurn); });
        yesButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCard); });
        hintButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCardHint); });
        grabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.GrabLandlord); });
        noGrabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.NoGrabLandlord); });

        EventCenter.AddListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    protected virtual void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    public virtual void SwitchPlayCard()
    {
        noButton.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        hintButton.gameObject.SetActive(true);
        grabButton.gameObject.SetActive(false);
        noGrabButton.gameObject.SetActive(false);
    }

    public virtual void SwitchGrabLandlord()
    {
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);
        grabButton.gameObject.SetActive(true);
        noGrabButton.gameObject.SetActive(true);
    }

    public virtual void SwitchReady()
    {
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);
        grabButton.gameObject.SetActive(false);
        noGrabButton.gameObject.SetActive(false);
    }

    public virtual void DisableButtons()
    {
        noButton.interactable = false;
        yesButton.interactable = false;
        hintButton.interactable = false;
        grabButton.interactable = false;
        noGrabButton.interactable = false;
    }

    public virtual void EnableButtons()
    {
        noButton.interactable = true;
        yesButton.interactable = true;
        hintButton.interactable = true;
        grabButton.interactable = true;
        noGrabButton.interactable = true;
    }

    protected void OnSelfEnterRoom()
    {
        ShowNoAnim();
        SwitchReady();
    }
}
