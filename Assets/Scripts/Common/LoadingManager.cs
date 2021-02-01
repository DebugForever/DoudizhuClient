using UnityEngine;

public static class LoadingManager
{
    public static void LoadSceneByLoadingPanel(string sceneName)
    {
        GameObject loadingPanelPrefab = ResourceManager.GetLoadingPanel();
        GameObject loadingPanel = GameObject.Instantiate(loadingPanelPrefab);
        loadingPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
        loadingPanel.GetComponent<LoadingPanel>().LoadScene(sceneName);
    }

}
