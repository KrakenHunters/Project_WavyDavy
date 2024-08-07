 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //[field: SerializeField] public GameMode CurrentGameMode => currentGameMode;
    private GameMode currentGameMode;

    private void Awake()
    {
        if(Instance != this)
        {
               Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void Initialize()
    {

    }

    public void StartGame(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }

    public enum GameMode
    {
        Career,
        Endless
    }
}
