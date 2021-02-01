using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPanel : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Button backButton;
    private UnityEngine.UI.InputField userNameField;
    private UnityEngine.UI.InputField passwordField;
    private UnityEngine.UI.Button registerButton;
    // === auto generated code end === 

    private void Awake()
    {
        // === auto generated code begin === 
        backButton = transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>();
        userNameField = transform.Find("UserNameField").GetComponent<UnityEngine.UI.InputField>();
        passwordField = transform.Find("PasswordField").GetComponent<UnityEngine.UI.InputField>();
        registerButton = transform.Find("RegisterButton").GetComponent<UnityEngine.UI.Button>();
        // === auto generated code end === 

        //register callbacks
        backButton.onClick.AddListener(OnBackBtnClicked);
        registerButton.onClick.AddListener(OnRegisterBtnClicked);
        EventCenter.AddListener(EventType.UIShowRegister, Show);

        Hide();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.UIShowRegister, Show);
    }

    private void OnRegisterBtnClicked()
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
            NetMsgCenter.instance.SendRegisterMsg(userNameField.text, passwordField.text);
        }
    }

    private void OnBackBtnClicked()
    {
        Hide();
        EventCenter.BroadCast(EventType.UIShowLogin);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
