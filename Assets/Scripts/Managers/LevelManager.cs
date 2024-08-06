 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [field: SerializeField] public int CurrentLevel { get; private set; }

    [SerializeField] private List<SurferInfoSO> contestantSurfers = new();

    private GameMode currentGameMode;

    private void Awake()
    {
        if(Instance != this)
        {
               Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);


        Initialize();
    }

    private void Initialize()
    {

    }

    void Update()
    {
        
    }

    private enum GameMode
    {
        Career,
        Endless
    }
}
