using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSceneManager : SingletonMono<GSceneManager>
{
    public GameObject mainRoot;

    public void LoadSceneAsync(int i)
    {
        StartCoroutine(LoadSceneCoroutine(i));
    }

    private IEnumerator LoadSceneCoroutine(int i)
    {
        // 开始异步加载
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = true;
        while (!asyncOp.isDone)
        {
            //Debug.Log("Loading progress: " + asyncOp.progress);
            yield return null;
        }
        //Debug.Log("Scene loaded!");
        mainRoot.SetActive(false);
    }

    public void StartBattleScene()
    {
        LoadSceneAsync(1);
    }
}