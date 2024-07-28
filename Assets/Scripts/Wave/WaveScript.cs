using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WaveScript : MonoBehaviour
{
    public GameEvent Event;

    private Animator _animator;
    private GamePhase currentPhase;

    private float animationSpeed;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
        Event.OnFlowChange.AddListener(FlowChange);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
    }

    private void Update()
    {
        _animator.speed = animationSpeed; //Check if needs any multiplier
    }

    private void FlowChange(float currentFlow)
    {
        animationSpeed = currentFlow;
    }

    private void SetPhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                if (currentPhase == GamePhase.Phase2)
                {
                    _animator.SetTrigger("Phase2To1");
                }

                break;
            case GamePhase.Phase2:
                if (currentPhase == GamePhase.Phase1)
                {
                    _animator.SetTrigger("Phase1To2");
                }
                else if (currentPhase == GamePhase.Phase3)
                {
                    _animator.SetTrigger("Phase3To2");
                }

                break;
            case GamePhase.Phase3:
                _animator.SetTrigger("Phase2To3");

                break;
            case GamePhase.Trick:
                //_animator.SetTrigger("Phase3ToTrick");

                break;

        }
        currentPhase = newPhase;
    }
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
