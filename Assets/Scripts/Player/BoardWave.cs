using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardWave : MonoBehaviour
{
    public GameEvent Event;
    [SerializeField] private Ease Ease;

    [SerializeField] private GameObject FrontWave;
    [SerializeField] private GameObject BackWave;

    private Animator animator;
    private Tween currentTween;
    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(EnableWave);
        Event.OnFlowChange.AddListener(ScaleAndSpeed);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.AddListener(EnableWave);
        Event.OnFlowChange.AddListener(ScaleAndSpeed);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void ScaleAndSpeed(float flow)
    {
        if (currentTween != null)
            currentTween.Kill();

        float newScale = Mathf.Lerp(0.6f, 2.4f, flow / 20f);

        currentTween = transform.DOScale(new Vector3(newScale, newScale, newScale), 0.5f).SetEase(Ease);
        float newSpeed = Mathf.Lerp(0.6f, 2.4f, flow / 20f);
        animator.speed = newSpeed;
    }

    private void EnableWave(GamePhase gamePhase)
    {
        switch (gamePhase)
        {
            case GamePhase.Trick:
                FrontWave.SetActive(false);
                BackWave.SetActive(false);
                break;
            default:
                FrontWave.SetActive(true);
                BackWave.SetActive(true);
                break;
        }
    }



}
