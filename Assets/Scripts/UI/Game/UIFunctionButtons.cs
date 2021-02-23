using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctionButtons : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button noButton;
    private UnityEngine.UI.Button yesButton;
    private UnityEngine.UI.Button hintButton;
    // === auto generated code end === 

    private void Awake()
    {
        // === auto generated code begin === 
        noButton = transform.Find("NoButton").GetComponent<UnityEngine.UI.Button>();
        yesButton = transform.Find("YesButton").GetComponent<UnityEngine.UI.Button>();
        hintButton = transform.Find("HintButton").GetComponent<UnityEngine.UI.Button>();
        // === auto generated code end === 

        //广播事件
        noButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PassTurn); });
        yesButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCard); });
        hintButton.onClick.AddListener(() => { EventCenter.BroadCast(EventType.PlayCardHint); });

    }

    public void DisableButtons()
    {
        noButton.interactable = false;
        yesButton.interactable = false;
        hintButton.interactable = false;
    }

    public void EnableButtons()
    {
        noButton.interactable = true;
        yesButton.interactable = true;
        hintButton.interactable = true;
    }
}
