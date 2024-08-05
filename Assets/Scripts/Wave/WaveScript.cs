using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WaveScript : MonoBehaviour
{
    public GameEvent Event;

    private Animator _animator;
    private GamePhase currentPhase;

    protected static readonly int Wave1To2Hash = Animator.StringToHash("TransitionPhase1ToPhase2");
    protected static readonly int Wave2To1Hash = Animator.StringToHash("TransitionPhase2ToPhase1");
    protected static readonly int Wave2To3Hash = Animator.StringToHash("TransitionPhase2ToPhase3");
    protected static readonly int Wave3To2Hash = Animator.StringToHash("TransitionPhase3ToPhase2");
    protected static readonly int IdleHash = Animator.StringToHash("Idle");


    [SerializeField]
    private GameObject waveAnimatorObj;
    [SerializeField]
    private GameObject parallaxObj;

    [SerializeField]
    private Material smallWaveMaterial;
    [SerializeField]
    private Material mediumWaveMaterial;
    [SerializeField]
    private Material largeWaveMaterial;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
        Event.OnFinishTransition.AddListener(DisableAnimator);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
        Event.OnFinishTransition.RemoveListener(DisableAnimator);


    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {

    }

    private void SetPhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:

                if (currentPhase == GamePhase.Phase2)
                {
                    parallaxObj.SetActive(false);
                    parallaxObj.GetComponent<MeshRenderer>().material = smallWaveMaterial;

                    _animator.CrossFade(Wave2To1Hash,0.2f);
                }
                currentPhase = newPhase;
                break;
            case GamePhase.Phase2:
                parallaxObj.SetActive(false);
                parallaxObj.GetComponent<MeshRenderer>().material = mediumWaveMaterial;

                if (currentPhase == GamePhase.Phase1)
                {
                    _animator.CrossFade(Wave1To2Hash, 0.2f);
                }
                else if (currentPhase == GamePhase.Phase3)
                {
                    _animator.CrossFade(Wave3To2Hash, 0.2f);
                }
                currentPhase = newPhase;

                break;
            case GamePhase.Phase3:
                if (currentPhase == GamePhase.Phase2)
                {
                    parallaxObj.SetActive(false);
                    parallaxObj.GetComponent<MeshRenderer>().material = largeWaveMaterial;

                    _animator.CrossFade(Wave2To3Hash, 0.2f);
                }
                currentPhase = newPhase;

                break;
            case GamePhase.Trick:
                //_animator.SetTrigger("Phase3ToTrick");
                currentPhase = newPhase;

                break;

        }
        currentPhase = newPhase;
    }

    private void DisableAnimator()
    {
        _animator.CrossFade(IdleHash, 0f);
        parallaxObj.SetActive(true);

    }
}
