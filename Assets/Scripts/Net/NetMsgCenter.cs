using ServerProtocol.Code;
using ServerProtocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerProtocol;
using System;
using System.Net.Sockets;
using ServerProtocol.SharedCode;

/// <summary>
/// 负责管理网络消息的发送类，做成单例类
/// </summary>
public class NetMsgCenter : MonoSingleton<NetMsgCenter>
{
    private class CallbackInfo
    {
        public Action<object> callback;
        public int callCount;//只检查==0，设置成-1表示不限制

        public CallbackInfo(Action<object> callback, int callCount)
        {
            this.callback = callback;
            this.callCount = callCount;
        }
    }

    public ClientPeer client { get; private set; }
    private readonly ChatModule chatModule = new ChatModule();
    private readonly PlayModule playModule = new PlayModule();
    private readonly MatchModule matchModule = new MatchModule();
    private readonly AccountModule accountModule = new AccountModule();

    private Dictionary<Tuple<int, int>, List<CallbackInfo>> CallbackDict = new Dictionary<Tuple<int, int>, List<CallbackInfo>>();

    protected override void Init()
    {
        base.Init();
        client = new ClientPeer();
        client.Connect("127.0.0.1", 6666);
        DontDestroyOnLoad(gameObject);
    }

    #region unity消息

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
    public void ListenServerMsgOnce(int opCode, int subOpCode, Action<object> callback)
    {
        ListenServerMsg(opCode, subOpCode, callback, 1);
    }

    /// <summary>
    /// 添加一个监听，回调会在下一次收到对应的服务器消息时触发。
    /// 回调触发listenCount次后移除
    /// </summary>
    /// <param name="listenCount">回调触发次数，-1表示永不移除</param>
    public void ListenServerMsg(int opCode, int subOpCode, Action<object> callback, int listenCount = -1)
    {
        if (listenCount == 0) // 0次直接不添加
            return;
        Tuple<int, int> key = new Tuple<int, int>(opCode, subOpCode);
        var value = new CallbackInfo(callback, listenCount);
        if (!CallbackDict.ContainsKey(key))
            CallbackDict.Add(key, new List<CallbackInfo>());
        CallbackDict[key].Add(value);
    }

    public void UnListenServerMsg(int opCode, int subOpCode, Action<object> callback)
    {
        Tuple<int, int> key = new Tuple<int, int>(opCode, subOpCode);
        if (CallbackDict.ContainsKey(key))
            CallbackDict[key].RemoveAll((item) => item.callback == callback);
    }

    /// <summary>
    /// 如果当前消息可以触发ListenNetMsgOnce的监听，那么触发一次并检查次数是否用尽。
    /// </summary>
    /// <remarks>因为-1表示永不移除，所以不检查负数</remarks>
    private void TryTriggerListener(NetMsg msg)
    {
        Tuple<int, int> key = new Tuple<int, int>(msg.opCode, msg.subOpCode);
        if (CallbackDict.ContainsKey(key))
        {
            foreach (var item in CallbackDict[key])
            {
                item.callback(msg.value);
                item.callCount -= 1;
            }
            CallbackDict[key].RemoveAll((item) => item.callCount == 0); //不会添加0次次数的监听，所以可以先执行再移除
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

    #region 发送特定消息（之后可能会单独拿出来）
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

    public void SendEnterRoomMsg()
    {
        SendNetMsg(OpCode.match, MatchCode.EnterCReq, null);
    }

    public void SendReadyMsg(bool ready)
    {
        SendNetMsg(OpCode.match, MatchCode.ReadyCReq, ready);
    }

    public void SendGrabLandlordMsg(bool isGrab)
    {
        SendNetMsg(OpCode.play, PlayCode.GrabLandlordCReq, isGrab);
    }

    public void SendPlayCardMsg(CardSet cardSet)
    {
        SendNetMsg(OpCode.play, PlayCode.PlayCardCReq, cardSet);
    }

    public void SendPassTurnMsg()
    {
        SendNetMsg(OpCode.play, PlayCode.PlayCardCReq, null);
    }


    #endregion
}
