using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSelect : MonoBehaviour {
    public Image startIcon;
    public Image quitIcon;

    MgrGame mgrGame;


    void Start()
    {
        mgrGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MgrGame>();
    }

    public void GameStart()
    {
        mgrGame.GameStatus = MgrGame.GAMESTATUS.LOADING_SCENE;
    }

    public void GameQuit()
    {
        Debug.Log("game quit");
    }
}
