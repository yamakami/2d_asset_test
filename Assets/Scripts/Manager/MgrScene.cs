using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MgrScene : MgrBase
{
    public GameObject canvasFaderBlack;

    [System.NonSerialized]
    public bool isFading = false;
    [System.NonSerialized]
    public CanvasGroup faderBlack;
    [System.NonSerialized]
    public float fadeDuration = 1f;
    [System.NonSerialized]
    public readonly float fadeOut = 1f;
    [System.NonSerialized]
    public readonly float fadeIn = 0f;

    readonly string managerScene = "ManagerScene";

    protected override void Start()
    {
        base.Start();

        faderBlack = canvasFaderBlack.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update () {

        switch (mgrGame.GameStatus)
        {
            case MgrGame.GAMESTATUS.LOADING_SCENE:
                if (!isFading)
                    StartCoroutine(FadeCoroutine(0f, LoadPlay));

                break;
        }
    }

    void LoadPlay()
    {
        StartInputBlock();
        mgrGame.GameStatus = MgrGame.GAMESTATUS.PLAY;
        StartCoroutine(UnloadSceneCoroutine());
        StartCoroutine(LoadSceneCoroutine("SampleScene"));
        Invoke("StopInputBlock", 2f);
    }

    public delegate void SceneLoadCallback();
 
    public IEnumerator FadeCoroutine(float waitTime, SceneLoadCallback sceneCallback)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        if (!isFading)
        {
            yield return FadeScenCoroutine(faderBlack, fadeOut);

            sceneCallback();

            yield return FadeScenCoroutine(faderBlack, fadeIn);
        }
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    public IEnumerator UnloadSceneCoroutine()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == managerScene)
            yield return null;

        yield return SceneManager.UnloadSceneAsync(sceneName);
    }

    public IEnumerator FadeScenCoroutine(CanvasGroup cvg, float fadeTo)
    {
        cvg.blocksRaycasts = true;
        isFading = true;

        float fadeSpeed = Mathf.Abs(cvg.alpha - fadeTo) / fadeDuration;

        while (!Mathf.Approximately(cvg.alpha, fadeTo))
        {
            yield return cvg.alpha = Mathf.MoveTowards(cvg.alpha, fadeTo, fadeSpeed * Time.deltaTime);
        }
        cvg.blocksRaycasts = false;
        isFading = false;
    }
}