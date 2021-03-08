using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionButtonsOnline : UIFunctionButtons
{
    // === auto generated code begin === 
    protected UnityEngine.UI.Button readyButton;
    protected UnityEngine.UI.Text readyButtonText;
    // === auto generated code end === 

    /// <summary>准备按钮的状态，true表示现在是[准备]</summary>
    protected bool readyBtnStatus = true;

    protected override void Awake()
    {
        base.Awake();
        // === auto generated code begin === 
        readyButton = transform.Find("ReadyButton").GetComponent<UnityEngine.UI.Button>();
        readyButtonText = transform.Find("ReadyButton/Text").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 

        readyButton.onClick.AddListener(OnReadyBtnClicked);

        EventCenter.AddListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    protected override void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.SelfEnterRoom, OnSelfEnterRoom);
    }

    protected void OnReadyBtnClicked()
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

    public override void SwitchPlayCard()
    {
        base.SwitchPlayCard();
        readyButton.gameObject.SetActive(false);
    }

    public override void SwitchGrabLandlord()
    {
        base.SwitchGrabLandlord();
        readyButton.gameObject.SetActive(false);
    }

    public override void SwitchReady()
    {
        base.SwitchReady();
        readyButton.gameObject.SetActive(true);
    }

    public override void DisableButtons()
    {
        base.DisableButtons();
        readyButton.interactable = false;
    }

    public override void EnableButtons()
    {
        base.EnableButtons();
        readyButton.interactable = true;
    }

}
