using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrGame : MgrBase
{
    public static bool inputBlock;

    GAMESTATUS gameStatus;
    public enum GAMESTATUS
    {
        NONE,
        START,
        PLAY,
        OVER,
        LOADING_SCENE,
        PLAYER_RESET,
    }

    void Awake()
    {

        //gameStatus = GAMESTATUS.START;
        gameStatus = GAMESTATUS.PLAY;
    }
	
	// Update is called once per frame
	void Update () {
        switch(gameStatus)
        {
            case GAMESTATUS.START: break;
            case GAMESTATUS.PLAY: break;
            case GAMESTATUS.OVER: break;
            case GAMESTATUS.LOADING_SCENE: break;
            case GAMESTATUS.PLAYER_RESET: break;
        }
    }

    public GAMESTATUS GameStatus
    {
        get
        {
            return gameStatus;
        }
        set
        {
            gameStatus = value;
        }
    }
}
