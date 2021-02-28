using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ServerProtocol.Code;

public class LoginPanel : HideablePanel
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button registerButton;
    private UnityEngine.UI.Button loginButton;
    private UnityEngine.UI.Button playOfflineButton;
    private UnityEngine.UI.InputField passwordField;
    private UnityEngine.UI.InputField userNameField;
    // === auto generated code end === 



    void Awake()
    {
        // === auto generated code begin === 
        registerButton = transform.Find("RegisterButton").GetComponent<UnityEngine.UI.Button>();
        loginButton = transform.Find("LoginButton").GetComponent<UnityEngine.UI.Button>();
        playOfflineButton = transform.Find("PlayOfflineButton").GetComponent<UnityEngine.UI.Button>();
        passwordField = transform.Find("PasswordField").GetComponent<UnityEngine.UI.InputField>();
        userNameField = transform.Find("UserNameField").GetComponent<UnityEngine.UI.InputField>();
        // === auto generated code end === 

        //register callbacks
        registerButton.onClick.AddListener(OnRegisterBtnClicked);
        loginButton.onClick.AddListener(OnLoginBtnClicked);
        playOfflineButton.onClick.AddListener(OnPlayOfflineButtonClicked);

        EventCenter.AddListener(EventType.UIShowLogin, Show);

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.UIShowLogin, Show);
    }

    void OnRegisterBtnClicked()
    {
        HideNoAnim();
        EventCenter.BroadCast(EventType.UIShowRegister);
    }

    void OnLoginBtnClicked()
    {
        if (userNameField == null || userNameField.text == "")
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "用户名为空");
        }
        else if (passwordField == null || passwordField.text == "")
        {
            EventCenter.BroadCast(EventType.UIFlashHint, "密码为空");
        }
        else
        {
            NetMsgCenter.instance.SendLoginMsg(userNameField.text, passwordField.text);
        }
    }

    void OnPlayOfflineButtonClicked()
    {
        LoadingManager.LoadSceneByLoadingPanel(Constants.SceneName.GameOffline);
    }

}
