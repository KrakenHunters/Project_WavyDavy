using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticlePlay : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private GameEvent Event;

    private bool stateCheck;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(BoolCheck);

        Event.OnIsTrickPossible += ImageCheck;
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(BoolCheck);

        Event.OnIsTrickPossible -= ImageCheck;
    }
    private void Start()
    {
        _particleSystem.gameObject.SetActive(true);
    }
    void Update()
    {
        if (GetComponent<CanvasGroup>().alpha >= 1f && !_particleSystem.isPlaying)
        {
            _particleSystem.gameObject.SetActive(true);
            _particleSystem.Play();
        }
        else if (GetComponent<CanvasGroup>().alpha < 1f && _particleSystem.isPlaying)
        {
            _particleSystem.Stop();
            _particleSystem.gameObject.SetActive(false);
        }
    }

    private void BoolCheck(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                stateCheck = false;
                break;
            case GamePhase.Phase2:
                stateCheck = true;
                break;
            case GamePhase.Phase3:
                stateCheck = true;
                break;
            case GamePhase.Trick:
                stateCheck = false;
                break;
        }

    }

    private void ImageCheck(bool check)
    {
        if (!check && stateCheck)
        {
            GetComponent<UIAnimator>().FadeInAnimate(true);
        }
        if (!stateCheck)
        {
            GetComponent<UIAnimator>().FadeInAnimate(false);
        }
    }
}
 