using ServerProtocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button rankBtn;
    private UnityEngine.UI.Button payBtn;
    // === auto generated code end === 


    // Start is called before the first frame update
    void Awake()
    {
        // === auto generated code begin === 
        rankBtn = transform.Find("ShortcutPanel/RankBtn").GetComponent<UnityEngine.UI.Button>();
        payBtn = transform.Find("ShortcutPanel/PayBtn").GetComponent<UnityEngine.UI.Button>();
        // === auto generated code end === 
        rankBtn.onClick.AddListener(() =>
        {
            NetMsgCenter.instance.SendNetMsg(OpCode.account, AccountCode.getRankListCReq, null);
            EventCenter.BroadCast(EventType.UIShowRankList);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
