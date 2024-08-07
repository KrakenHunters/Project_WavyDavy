using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameDataSO gameDataSO;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnEndlessGame()
    {
        gameDataSO.gameMode = GameMode.Endless;
    }

    public void OnCareerGame()
    {
        gameDataSO.gameMode = GameMode.Career;
    }
}

public enum GameMode
{
    Career,
    Endless
}