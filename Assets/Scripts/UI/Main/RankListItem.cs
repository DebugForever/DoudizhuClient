using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankListItem : MonoBehaviour
{
    // === auto generated code begin === 
    private UnityEngine.UI.Text number;
    private UnityEngine.UI.Text usernameText;
    private UnityEngine.UI.Text scoreText;
    // === auto generated code end === 

    void Awake()
    {
        // === auto generated code begin === 
        number = transform.Find("RankLabel/Number").GetComponent<UnityEngine.UI.Text>();
        usernameText = transform.Find("UsernameText").GetComponent<UnityEngine.UI.Text>();
        scoreText = transform.Find("ScoreText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 
    }

    public void SetInfo(int rank,string username,int score)
    {
        number.text = rank.ToString();
        usernameText.text = username;
        scoreText.text = string.Format("{0}金币", score);
    }
}
