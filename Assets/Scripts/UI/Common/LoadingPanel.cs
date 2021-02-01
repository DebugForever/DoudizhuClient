using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{

    // === auto generated code begin === 
    private UnityEngine.UI.Text loadingText;
    private UnityEngine.UI.Text loadingPrecentText;
    // === auto generated code end === 

    public void LoadScene(string SceneName)
    {
        // === auto generated code begin === 
        loadingText = transform.Find("LoadingText").GetComponent<UnityEngine.UI.Text>();
        loadingPrecentText = transform.Find("LoadingPrecentText").GetComponent<UnityEngine.UI.Text>();
        // === auto generated code end === 

        loadingText.text = "加载中";
        loadingText.DOText("加载中...", 1.0f).SetLoops(-1, LoopType.Restart);

        StartCoroutine(LoadSceneGenerator(SceneName));
    }

    public void SetUiProgress(float percent)
    {
        loadingPrecentText.text = string.Format("{0:p1}", percent);//p表示百分比格式化
    }

    private IEnumerator LoadSceneGenerator(string SceneName)
    {
        const float step = 0.3f;
        float shownProgress = 0f;//实际显示的进度
        const float displayDeltaTime = 0.5f;


        AsyncOperation ao = SceneManager.LoadSceneAsync(SceneName);
        ao.allowSceneActivation = false;//禁用加载完成自动切换
        GameObject loadingPanel = GameObject.Instantiate(ResourceManager.GetLoadingPanel());

        while (shownProgress < 0.9f)//如果禁用加载完成自动切换，那么进度最多到0.9
        {
            //简单的追及问题
            if (shownProgress + step < ao.progress)
            {
                shownProgress += step;
            }
            else
            {
                shownProgress = ao.progress;
            }

            SetUiProgress(shownProgress);
            //yield return new WaitForEndOfFrame();//每帧更新的话，跳的太快了，限制一下频率
            yield return new WaitForSeconds(displayDeltaTime);//每0.5s更新一次显示进度
        }

        SetUiProgress(1.0f);
        DOTween.Clear();//清除所有DOTween，因为DOTween在跨场景时仍会执行，但是对应物体已销毁。
        ao.allowSceneActivation = true;
    }
}
