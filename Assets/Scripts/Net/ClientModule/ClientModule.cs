/// <summary>
/// NetMsgCenter也改成了观察者模式（消息分发），
/// 所以这个类的意义就不大了
/// </summary>
public abstract class ClientModule
{
    public abstract void OnReceiveNetMsg(int subOpCode, object value);
}

