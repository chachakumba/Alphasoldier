using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    float timeToTransitionSceneAppear = 1;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeSceneCour(sceneName));
    }
    IEnumerator ChangeSceneCour(string sceneName)
    {
        Scene sceneToDisable = SceneManager.GetActiveScene();
        SceneManager.LoadScene("TransitionScene", LoadSceneMode.Additive);
        yield return null;
        LeanTween.alphaCanvas(TransitionSceneManager.instance.mainCanvasGroup, 1, timeToTransitionSceneAppear);
        yield return new WaitForSeconds(timeToTransitionSceneAppear);
        //yield return new WaitUntil(() => SceneManager.UnloadSceneAsync(sceneToDisable).isDone);
        AsyncOperation sceneTransition = SceneManager.UnloadSceneAsync(sceneToDisable);
        yield return new WaitUntil(() => sceneTransition.isDone);
        sceneTransition = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => sceneTransition.isDone);
        LeanTween.alphaCanvas(TransitionSceneManager.instance.mainCanvasGroup, 0, timeToTransitionSceneAppear).setOnComplete(()=> { SceneManager.UnloadScene("TransitionScene"); });
    }
}
