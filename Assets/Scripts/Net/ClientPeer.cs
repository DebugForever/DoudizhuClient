using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using ServerProtocol;

public class ClientPeer
{
    private const int BufSize = 1024;
    private Socket clientSocket;
    private byte[] receiveBuffer = new byte[BufSize];

    /// <summary>
    /// 暂存收到的字节流
    /// </summary>
    private List<byte> cache = new List<byte>();

    /// <summary>
    /// 接收到的消息，以队列形式存储
    /// </summary>
    private Queue<NetMsg> msgQueue = new Queue<NetMsg>();

    /// <summary>
    /// 可复用的仅发送用消息对象
    /// </summary>
    private NetMsg msg = new NetMsg();

    public ClientPeer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip">服务器ip</param>
    /// <param name="port">服务器端口</param>
    public void Connect(string ip, int port)
    {
        try
        {
            clientSocket.Connect(ip, port);
            Debug.Log("连接服务器成功");
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void StartReceive()
    {
        if (clientSocket != null && clientSocket.Connected == true)
        {
            clientSocket.BeginReceive(receiveBuffer, 0, BufSize, SocketFlags.None, ReceiveCallback, null);
        }
        else
        {
            Debug.LogWarning("与服务器的连接丢失，接收消息失败");
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        int length = clientSocket.EndReceive(ar);
        byte[] packet = new byte[length];
        Buffer.BlockCopy(receiveBuffer, 0, packet, 0, length);
        cache.AddRange(packet);

        try
        {
            ProcessReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }



        StartReceive();
    }

    /// <summary>
    /// 处理收到的数据，转化为netmsg
    /// </summary>
    private void ProcessReceive()
    {
        while (true)
        {
            byte[] data = EncodingTools.Decode(ref cache);
            if (data == null)
                break;
            msgQueue.Enqueue(NetMsg.Deserialize(data));
        }
    }

    /// <summary>
    /// 向服务器发送数据
    /// </summary>
    /// <param name="opCode">主操作码</param>
    /// <param name="subOpCode">副操作码</param>
    /// <param name="value">传递的对象</param>
    public void SendNetMsg(int opCode, int subOpCode, object value)
    {
        msg.Reset(opCode, subOpCode, value);
        SendMsg(msg);
    }

    private void SendMsg(NetMsg msg)
    {
        byte[] data = EncodingTools.Encode(msg.Serialize());
        try
        {
            clientSocket.Send(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 取出并移除消息队列中的一条消息
    /// </summary>
    /// <returns>消息队列第一条消息，消息队列为空返回null</returns>
    public NetMsg PopMsg()
    {
        if (msgQueue.Count == 0)
            return null;

        NetMsg msg = msgQueue.Dequeue();
        return msg;
    }
}
