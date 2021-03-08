using ServerProtocol.Dto;
using ServerProtocol.Code;
using MyModel;
using UnityEngine;

public class PlayModule : ClientModule
{
    public override void OnReceiveNetMsg(int subOpCode, object value)
    {
        foreach (var item in typeof(PlayCode).GetFields())
        {
            if (subOpCode == (int)item.GetRawConstantValue())
                Debug.Log($"playModule receive message subcode={item.Name}");
        }
    }
}

