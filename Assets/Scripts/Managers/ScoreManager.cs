using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreUIHandler scoreUIHandler;

    public int Score { get; private set; }
    public GameEvent Event;

    private void OnEnable()
    {
        Event.OnTrickComplete += AddScore;
    }

    private void OnDisable()
    {
        Event.OnTrickComplete -= AddScore;
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
        Score += pth.CurrentTrick.Points;
        scoreUIHandler.UpdateScore(Score);
    }
}
