using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameView : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => { EventCenter.BroadCast(EventType.TestEvent); });
    }


}
