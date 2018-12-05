using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MgrCanvas : MgrBase
{
    public Player player;

    MgrScene mgrScene;

    GameObject canvas;
    GameObject startPanel;
    GameObject playerPanel;
    GameObject gameOverPanel;

    Slider HPSlider;

    static int iniLifeAmount = 2;
    public static int lifeAmount = iniLifeAmount;
    List<Image> lifes = new List<Image>();

    protected override void Start()
    {
        base.Start();

        mgrScene = GameObject.Find("GameManager").transform.Find("SceneManager").GetComponent<MgrScene>();

        canvas = GameObject.Find("Canvas");

        // active falseも取得
        startPanel    = canvas.transform.Find("StartPanel").gameObject;
        playerPanel   = canvas.transform.Find("PlayerPanel").gameObject;
        gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;

        HPSlider = canvas.transform.Find("PlayerPanel/HP/HPSlider").GetComponent<Slider>();
        GameObject lifePanel = canvas.transform.Find("PlayerPanel/Life/LifePanel").gameObject;

        lifes.AddRange(lifePanel.GetComponentsInChildren<Image>(true));
        lifes.RemoveAt(0); // lifePanel分のImageを削除
    }

    void Update()
    {
        switch (mgrGame.GameStatus)
        {
            case MgrGame.GAMESTATUS.START:
                ShowStartPanel();

                HidePlayerPanel();
                HideGameOverPanel();
                HidePlayer();
                break;
            case MgrGame.GAMESTATUS.PLAY:
                HideStartPanel();

                ShowPlayerPanel();
                SetHPSliderValue();
                SetLifeAmount();
                ShowPlayer();
                break;
 
            case MgrGame.GAMESTATUS.OVER:
                player.gameObject.SetActive(false);
                ShowGameOverPanel();

                if(!mgrScene.isFading)
                    StartCoroutine(mgrScene.FadeCoroutine(2f, BackToStart));

                break;

            case MgrGame.GAMESTATUS.PLAYER_RESET:
                player.gameObject.SetActive(false);

                if (!mgrScene.isFading)
                    StartCoroutine(mgrScene.FadeCoroutine(0.5f, PlayerReset));

                break;
        }
    }

    void PlayerReset()
    {
        MgrGame.inputBlock = true;
        player.PlayerReset();
        mgrGame.GameStatus = MgrGame.GAMESTATUS.PLAY;

        Invoke("StopInputBlock", 1.5f);
    }

    void BackToStart()
    {
        player.PlayerReset();
        lifeAmount = iniLifeAmount;
        mgrGame.GameStatus = MgrGame.GAMESTATUS.START;
        StartCoroutine(mgrScene.UnloadSceneCoroutine());
    }

    void ShowStartPanel()
    {
        startPanel.SetActive(true);
    }

    void HideStartPanel()
    {
        startPanel.SetActive(false);
    }

    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    void ShowPlayerPanel()
    {
        if (playerPanel.activeSelf)
            return;

        playerPanel.SetActive(true);
    }

    void HidePlayerPanel()
    {
        playerPanel.SetActive(false);
    }

    void ShowPlayer()
    {
        player.gameObject.SetActive(true);
    }

    void HidePlayer()
    {
        if (player)
            player.gameObject.SetActive(false);
    }

    void SetHPSliderValue()
    {
        HPSlider.value = player.GetHitPoint();
    }

    void SetLifeAmount()
    {
        for (int i = 0; i < lifes.Count; i++)
        {
            lifes[i].gameObject.SetActive((i < lifeAmount) ? true : false);
        }
    }
}
