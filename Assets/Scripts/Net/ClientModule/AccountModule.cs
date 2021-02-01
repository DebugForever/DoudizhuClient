using ServerProtocol.Dto;
using ServerProtocol.Code;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class AccountModule : ClientModule
{
    public override void OnReceiveNetMsg(int subOpCode, object value)
    {
        switch (subOpCode)
        {
            case AccountCode.registerSRes:
                HandleRegisterSRes((int)value);
                break;
            case AccountCode.loginSRes:
                HandleLoginSRes((int)value);
                break;
            case AccountCode.getUserInfoSRes:
                HandleGetUserInfoSRes((UserInfoDto)value);
                break;
            case AccountCode.getRankListSRes:
                HandleGetRankListSRes((RankListDto)value);
                break;
            default:
                Debug.LogWarning("subOpCode not exist!");
                break;
        }
    }


    public AccountDto CreateAccountDto(string userName, string password)
    {
        HashAlgorithm hashAlgorithm = SHA256.Create();
        byte[] hashResult = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
        string base64Str = Convert.ToBase64String(hashResult);
        AccountDto dto = new AccountDto(userName, base64Str);
        return dto;
    }


    #region 处理服务器消息
    void HandleRegisterSRes(int responseCode)
    {
        if (responseCode == AccountReturnCode.success)
            EventCenter.BroadCast(EventType.UIFlashHint, "注册成功");
        else if (responseCode == AccountReturnCode.userExist)
            EventCenter.BroadCast(EventType.UIFlashHint, "用户名已存在");
        else
        {
            Debug.LogWarning("服务器返回了未知的注册返回码");
        }
    }

    void HandleLoginSRes(int responseCode)
    {
        switch (responseCode)
        {
            case AccountReturnCode.userNotFound:
                EventCenter.BroadCast(EventType.UIFlashHint, "用户名不存在");
                break;
            case AccountReturnCode.passwordNotMatch:
                EventCenter.BroadCast(EventType.UIFlashHint, "用户名与密码不匹配");
                break;
            case AccountReturnCode.userOnline:
                EventCenter.BroadCast(EventType.UIFlashHint, "用户已在线");
                break;
            case AccountReturnCode.success:
                //登录成功，获取当前登录的用户信息
                NetMsgCenter.instance.SendNetMsg(OpCode.account, AccountCode.getUserInfoCReq, null);//这个请求不需要发送对象
                EventCenter.BroadCast(EventType.UIFlashHint, "登录成功，加载用户信息。。。");
                NetMsgCenter.instance.ListenNetMsgOnce(OpCode.account, AccountCode.getUserInfoSRes, (NetMsg) =>
                {
                    LoadingManager.LoadSceneByLoadingPanel("Main");
                });//收到服务器发回的用户信息后再切换场景
                break;
            default:
                Debug.LogWarning("服务器返回了未知的登录返回码");
                break;
        }
    }

    private void HandleGetUserInfoSRes(UserInfoDto dto)
    {
        Models.gameModel.userInfoDto = dto;
    }

    private void HandleGetRankListSRes(RankListDto dto)
    {
        EventCenter.BroadCast(EventType.UISetRankList, dto);
    }

    #endregion

}

