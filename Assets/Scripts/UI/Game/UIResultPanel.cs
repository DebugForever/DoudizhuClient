using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResultPanel : HideablePanel
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text title;
    private UnityEngine.UI.Button resetBtn;
    private UnityEngine.UI.Button exitBtn;
    private UnityEngine.UI.Text coinTitle;
    private UnityEngine.UI.Text coinText;
    // === auto generated code end === 

    private void Awake()
    {
        // === auto generated code begin === 
        title = transform.Find("Title").GetComponent<UnityEngine.UI.Text>();
        resetBtn = transform.Find("Buttons/ResetBtn").GetComponent<UnityEngine.UI.Button>();
        exitBtn = transform.Find("Buttons/ExitBtn").GetComponent<UnityEngine.UI.Button>();
        coinTitle = transform.Find("CoinPanel/CoinTitle").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("CoinPanel/CoinText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 

        resetBtn.onClick.AddListener(() => { EventCenter.BroadCast(EventType.MatchReset); Hide(); });
        exitBtn.onClick.AddListener(() => { EventCenter.BroadCast(EventType.MatchExit); Hide(); });

        EventCenter.AddListener(EventType.MatchOver, Show);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.MatchOver, Show);
    }
}
