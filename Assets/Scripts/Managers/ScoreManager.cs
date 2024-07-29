using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreUIHandler scoreUIHandler;

    public int Score { get; private set; }
    public GameEvent Event;

    private void OnEnable()
    {
        Event.OnTrickFinish += AddScore;
    }

    private void OnDisable()
    {
        Event.OnTrickFinish -= AddScore;
    }

    void Start()
    {
        if (scoreUIHandler == null)
        {
           scoreUIHandler = GetComponent<ScoreUIHandler>();
        }
        Score = 0;
    }
    private void AddScore(PlayerTrickHandler pth)
    {
        if (pth.CurrentResult == Tricks.TrickResult.Complete)
        {
            Score += pth.CurrentTrick.Points;
            scoreUIHandler.UpdateScore(Score);
        }
    }
}
