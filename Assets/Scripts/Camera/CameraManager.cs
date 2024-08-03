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
            case GamePhase.Trick:
                animator.Play("TrickState");
                break;
            default: 
                animator.Play("BaseState");
                break;
        }
    }


}
