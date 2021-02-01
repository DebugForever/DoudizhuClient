using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoPanel : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Image userInfoBg;
    private UnityEngine.UI.Image headIconMask;
    private UnityEngine.UI.Image headIcon;
    private UnityEngine.UI.Text usernameTitle;
    private UnityEngine.UI.Text usernameText;
    private UnityEngine.UI.Text coinTitle;
    private UnityEngine.UI.Text coinText;
    // === auto generated code end === 


    private void Awake()
    {
        // === auto generated code begin === 
        userInfoBg = transform.Find("UserInfoBg").GetComponent<UnityEngine.UI.Image>();
        headIconMask = transform.Find("HeadIconMask").GetComponent<UnityEngine.UI.Image>();
        headIcon = transform.Find("HeadIconMask/HeadIcon").GetComponent<UnityEngine.UI.Image>();
        usernameTitle = transform.Find("UsernameTitle").GetComponent<UnityEngine.UI.Text>();
        usernameText = transform.Find("UsernameText").GetComponent<UnityEngine.UI.Text>();
        coinTitle = transform.Find("CoinTitle").GetComponent<UnityEngine.UI.Text>();
        coinText = transform.Find("CoinText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 

        if (Models.gameModel.userInfoDto != null)
        {
            usernameText.text = Models.gameModel.userInfoDto.username;
            coinText.text = Models.gameModel.userInfoDto.coin.ToString();
            headIcon.sprite = ResourceManager.GetHeadIcon(Models.gameModel.userInfoDto.iconName);
        }
        else
        {
            //没有收到或者过晚收到userInfoDto，这个弄成异步会更好
            Debug.LogWarning("Models.gameModel.userInfoDto == null");
        }
    }
}
