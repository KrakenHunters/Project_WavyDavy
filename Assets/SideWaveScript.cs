using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWaveScript : MonoBehaviour
{
    public GameEvent Event;

    protected static readonly int SmallWave = Animator.StringToHash("SmallWave");
    protected static readonly int MediumWave = Animator.StringToHash("MediumWave");
    protected static readonly int LargeWave = Animator.StringToHash("LargeWave");

    Animator animator;
    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ChangeWave);
        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ChangeWave);
    }

    void ChangeWave(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                animator.CrossFade(SmallWave, 0.5f);
                transform.DOLocalMove(new Vector3(1f,-2f, -1f), 2f);
                break;
            case GamePhase.Phase2:
                animator.CrossFade(MediumWave, 0.5f);
                transform.DOLocalMove(new Vector3(-1.5f, -2.2f, -1f), 2f);
                break;
            case GamePhase.Phase3:
                animator.CrossFade(LargeWave, 0.5f);
                transform.DOLocalMove(new Vector3(-4f, -1.3f, -1f), 2f);
                break;
        }

    }

    
}
