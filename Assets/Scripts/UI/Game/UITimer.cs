using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text timerText;
    // === auto generated code end === 
    private float timer;
    private int timeLeft;

    private void Awake()
    {
        // === auto generated code begin === 
        timerText = transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
        //gameObject.SetActive(false);
        StartTimer(60);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            timeLeft -= Mathf.FloorToInt(timer);
            timer -= Mathf.FloorToInt(timer);
            timerText.text = timeLeft.ToString();
            if (timeLeft <= 0)
            {
                StopTimer();
                EventCenter.BroadCast(EventType.PlayerTimeUp);
            }
        }
    }

    public void StartTimer(int timeLimit)
    {
        gameObject.SetActive(true);
        timer = 0;
        timeLeft = timeLimit;
    }

    public void StopTimer()
    {
        timeLeft = 0;
        timer = 0;
        gameObject.SetActive(false);
    }

}
