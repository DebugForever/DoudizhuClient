using ServerProtocol.Code;
using ServerProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol;
using System;

/// <summary>
/// 负责管理网络消息的发送类，做成单例类
/// </summary>
public class NetMsgCenter : MonoBehaviour
{
    public static NetMsgCenter instance { get; private set; }

    public ClientPeer client { get; private set; }
    private readonly ChatModule chatModule = new ChatModule();
    private readonly PlayModule playModule = new PlayModule();
    private readonly MatchModule matchModule = new MatchModule();
    private readonly AccountModule accountModule = new AccountModule();

    private Dictionary<Tuple<int, int>, Action<NetMsg>> CallbackOnceDict = new Dictionary<Tuple<int, int>, Action<NetMsg>>();

    #region unity消息
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            client = new ClientPeer();
            client.Connect("127.0.0.1", 6666);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //防止多次加载脚本导致冗余对象
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (client == null)
            return;

        while (true)
        {
            NetMsg msg = client.PopMsg();

            if (msg == null)
                break;

            HandleServerMsg(msg);
        }
    }
    #endregion

    /// <summary>
    /// 添加一个监听，回调会在下一次收到对应的服务器消息时触发并移除
    /// </summary>
    public void ListenNetMsgOnce(int opCode, int subOpCode, Action<NetMsg> action)
    {
        Tuple<int, int> key = new Tuple<int, int>(opCode, subOpCode);
        if (CallbackOnceDict.ContainsKey(key))
            CallbackOnceDict[key] += action;
        else
            CallbackOnceDict.Add(key, action);
    }

    /// <summary>
    /// 如果当前消息可以触发ListenNetMsgOnce的监听，那么触发一次并移除它
    /// </summary>
    private void TryTriggerListener(NetMsg msg)
    {
        Tuple<int, int> key = new Tuple<int, int>(msg.opCode, msg.subOpCode);
        if (CallbackOnceDict.ContainsKey(key))
        {
            CallbackOnceDict[key](msg);
            CallbackOnceDict.Remove(key);
        }
    }


    #region 基本收发信息
    private void HandleServerMsg(NetMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.account:
                accountModule.OnReceiveNetMsg(msg.subOpCode, msg.value);
                break;
            case OpCode.match:
                matchModule.OnReceiveNetMsg(msg.subOpCode, msg.value);
                break;
            case OpCode.chat:
                chatModule.OnReceiveNetMsg(msg.subOpCode, msg.value);
                break;
            case OpCode.play:
                playModule.OnReceiveNetMsg(msg.subOpCode, msg.value);
                break;
            default:
                Debug.LogWarningFormat("unrecongized net opcode {0}", msg.opCode);
                break;
        }
        TryTriggerListener(msg);
    }

    public void SendNetMsg(int opCode, int subOpCode, object value)
    {
        if (client != null)
            client.SendNetMsg(opCode, subOpCode, value);
        else
            Debug.LogError("client is null!");
    }
    #endregion

    public void SendRegisterMsg(string userName, string password)
    {
        AccountDto dto = accountModule.CreateAccountDto(userName, password);
        SendNetMsg(OpCode.account, AccountCode.registerCReq, dto);
    }

    public void SendLoginMsg(string userName, string password)
    {
        AccountDto dto = accountModule.CreateAccountDto(userName, password);
        SendNetMsg(OpCode.account, AccountCode.loginCReq, dto);
    }


}
