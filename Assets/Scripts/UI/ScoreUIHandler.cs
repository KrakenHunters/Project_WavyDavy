using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ScoreUIHandler : MonoBehaviour
{
    [Header("Player UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Opponent UI Elements")]
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private Image opponentImage;

    [SerializeField] private float countSpeed;

    private Tween countingTween; 

    private void Awake()
    {
        scoreText.text = "Score: 0";
        //opponentScoreText.text = "Opponent Score: 0";
    }

    public void UpdateScore(int score)
    {

        Debug.Log("Score: " + score);
        scoreText.text = $"Score: {score}";
    }
}
