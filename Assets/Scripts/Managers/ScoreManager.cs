using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private AudioClip countCoinClip;
    [SerializeField] private ScoreUIHandler scoreUIHandler;
    [SerializeField] private GameDataSO scoreSO;
    public int Score { get; private set; }
    public GameEvent Event;

    private int plusPointsForTime;

    private void OnEnable()
    {
        Event.OnTrickFinish += AddScore;
        Event.OnTrickRunning += CheckTimer;
    }

    private void OnDisable()
    {
        Event.OnTrickFinish -= AddScore;
        Event.OnTrickRunning += CheckTimer;

    }

    void Start()
    {
        if (scoreUIHandler == null)
        {
           scoreUIHandler = GetComponent<ScoreUIHandler>();
        }
        Score = 0;
        scoreSO.Score = Score;
    }
    private void AddScore(PlayerTrickHandler pth)
    {
        AudioManager.Instance.PlayAudio(countCoinClip);

        if (pth.CurrentResult == Tricks.TrickResult.Complete)
        {
            Score += pth.CurrentTrick.Points + plusPointsForTime;
            scoreSO.Score = Score;
            scoreUIHandler.UpdateScore(Score);
        }
    }

    private void CheckTimer(float trickTimer)
    {
        plusPointsForTime = Mathf.RoundToInt(trickTimer * 10f);
    }

}
