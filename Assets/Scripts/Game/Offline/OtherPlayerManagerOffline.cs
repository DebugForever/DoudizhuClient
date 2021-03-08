using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardManager = CardManagerOffline;//做在线模式的时候可以不用改代码
using ServerProtocol.SharedCode;
public class OtherPlayerManagerOffline : PlayerManagerOfflineBase
{
    [SerializeField] private float AiDelay = 1.0f;
    private new UIOtherPlayer view; //覆盖基类的这个，因为类型不同，这是抽象的必要代价

    protected override void Awake()
    {
        view = GetComponentInChildren<UIOtherPlayer>();
        base.view = view;
        gameManager = GetComponentInParent<GameManagerOffline>();
    }


    public override void StartTurn(TurnType turnType)
    {
        base.StartTurn(turnType);
        if (turnType == TurnType.PlayCard)
            StartCoroutine(WaitPlayCardCoroutine());
        else if (turnType == TurnType.GarbLandlord)
            StartCoroutine(WaitGrabLandlordCoroutine());
    }

    private IEnumerator WaitPlayCardCoroutine()
    {
        if (IsAI)
        {
            yield return new WaitForSeconds(AiDelay);
            CardSet result = ai.PlayCard(CardManager.LastHand);
            Debug.LogFormat("player{0} CardSet:{1}", playerIndex, result);
            if (result.Type == CardSetType.Invalid || result.Type == CardSetType.None)
            {
                PassTurn();
                yield break;
            }
            PlayCard(result);
        }
        else
        {
            //玩家控制，这里没有，直接不出。
            //或者等待回合结束也行
            yield return new WaitForSeconds(AiDelay);
            PassTurn();
        }

    }

    private IEnumerator WaitGrabLandlordCoroutine()
    {
        if (IsAI)
        {
            yield return new WaitForSeconds(AiDelay);
            EndTurnGrabLandlord(ai.GrabLandlord());
        }
        else
        {
            yield return new WaitForSeconds(AiDelay);
            EndTurnGrabLandlord(false);
        }
    }
}
