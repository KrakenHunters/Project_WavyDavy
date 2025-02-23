using UnityEngine;
using Utilities;

public class GameManager : Singleton<GameManager>
{

    [field: SerializeField] public float GameTime { get; private set; }
    public GamePhase currentGamePhase { get; private set; }

    public GameDataSO gameDataSO;
    public GameEvent Event;

    private CountdownTimer countdownTimer;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ChangeGamePhase);
        Event.OnFinishTransition.AddListener(Resume);

    }
    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ChangeGamePhase);
        Event.OnFinishTransition.RemoveListener(Resume);
    }
    private void Awake()
    {
        countdownTimer = new CountdownTimer(GameTime);
        Event.OnChangeGameState.Invoke(GamePhase.Phase1);
        countdownTimer.OnTimerStop += EndGame;
        gameDataSO.PlayedGame = true;
    }

    private void EndGame()
    {
        Event.OnUpdateGameTimer.Invoke(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Event.OnGameEnd.Invoke();
    }

    private void Start()
    {

        if (gameDataSO.gameMode == GameMode.Career)
        {
            Event.OnSetGameTimer?.Invoke(GameTime);
            countdownTimer.Start();
        }
        else if (gameDataSO.gameMode == GameMode.Endless)
        {
            Debug.Log("Endless Mode");
        }
    }

    private void ChangeGamePhase(GamePhase nextPhase)
    {
        countdownTimer.Pause();

        switch (nextPhase)
        {
            case GamePhase.Phase1:
                currentGamePhase = GamePhase.Phase1;
                break;
            case GamePhase.Phase2:
                currentGamePhase = GamePhase.Phase2;
                break;
            case GamePhase.Phase3:
                currentGamePhase = GamePhase.Phase3;
                break;
            case GamePhase.Trick:
                currentGamePhase = GamePhase.Trick;
                break;
        }
    }

    private void Update()
    {
        countdownTimer.Tick(Time.deltaTime);
        if (!countdownTimer.IsFinished)
            Event.OnUpdateGameTimer?.Invoke(countdownTimer.Progress * GameTime);
    }

    private void Resume()
    {
        if (currentGamePhase != GamePhase.Trick)
            countdownTimer.Resume();
    }

}

public enum GamePhase
{
    Phase1,
    Phase2,
    Phase3,
    Trick
}
