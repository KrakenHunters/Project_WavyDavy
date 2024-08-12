using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent Event;

    private Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        Event.OnChangeGameState.AddListener(SetPhase);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
    }

    private void SetPhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                animator.Play("PaddleState");
                break;
            case GamePhase.Phase2:
                animator.Play("Phase2State");
                break;
            case GamePhase.Phase3:
                animator.Play("Phase3State");
                break;
            case GamePhase.Trick:
                animator.Play("TrickState");
                break;
            default: 
                break;
        }
    }


}
