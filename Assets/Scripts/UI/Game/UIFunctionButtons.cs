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
    // === auto generated code end === 

    private void Awake()
    {
        // === auto generated code begin === 
        noButton = transform.Find("NoButton").GetComponent<UnityEngine.UI.Button>();
        yesButton = transform.Find("YesButton").GetComponent<UnityEngine.UI.Button>();
        hintButton = transform.Find("HintButton").GetComponent<UnityEngine.UI.Button>();
        grabButton = transform.Find("GrabButton").GetComponent<UnityEngine.UI.Button>();
        noGrabButton = transform.Find("NoGrabButton").GetComponent<UnityEngine.UI.Button>();
        // === auto generated code end === 

        //广播事件
        noButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PassTurn); });
        yesButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCard); });
        hintButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCardHint); });
        grabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.GrabLandlord); });
        noGrabButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.NoGrabLandlord); });

    }

    public void SwitchPlayCard()
    {
        noButton.gameObject.SetActive(true);
        yesButton.gameObject.SetActive(true);
        hintButton.gameObject.SetActive(true);
        grabButton.gameObject.SetActive(false);
        noGrabButton.gameObject.SetActive(false);
    }

    public void SwitchGrabLandlord()
    {
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);
        grabButton.gameObject.SetActive(true);
        noGrabButton.gameObject.SetActive(true);
    }

    public void DisableButtons()
    {
        noButton.interactable = false;
        yesButton.interactable = false;
        hintButton.interactable = false;
        grabButton.interactable = false;
        noGrabButton.interactable = false;
    }

    public void EnableButtons()
    {
        noButton.interactable = true;
        yesButton.interactable = true;
        hintButton.interactable = true;
        grabButton.interactable = true;
        noGrabButton.interactable = true;
    }
}
