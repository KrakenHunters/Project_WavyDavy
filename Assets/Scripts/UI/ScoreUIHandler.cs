using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUIHandler : MonoBehaviour
{
    [Header("Player UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private float countSpeed;


    private float currentScore;
    private Tween countingTween;

    private void Awake()
    {
        scoreText.text = "Score: 0";
    }

    public void UpdateScore(int score)
    {
        countingTween = DOTween.To(() => currentScore, x => currentScore = x, score, countSpeed);// animate the score
    }

    private void Update() => scoreText.text = $"Score: {(int)currentScore}";
}
