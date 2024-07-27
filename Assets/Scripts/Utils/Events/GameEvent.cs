using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{

    public UnityEvent<GamePhase> OnChangeGameState;
    public UnityEvent OnFinishTransition;

    public UnityEvent<GameObject> OnFlowBarAbove;
    public UnityEvent<GameObject> OnFlowBarBelow;


    // Flow Events
    public UnityEvent<float> OnHitObject;
    public UnityEvent<WaterObject> OnReachDeadZone;

    public UnityEvent<float> OnIncreaseFlow;

    //public UnityEvent<GameObject> OnLoseFlow;
    //public UnityEvent<float> OnGainFlow;

    public UnityEvent OnStartTrick;
    public UnityEvent<bool> OnEndTrick;

    public UnityEvent<GameObject> OnCelebration;

    public UnityEvent<GameObject> OnGameEnd;

    public UnityEvent OnUIUpdate;


    public UnityAction<TrickCombo> OnPlayerInput;


    public UnityAction<List<TrickSO>> OnTrickInput;
    public UnityAction<TrickManager> OnTrickStart;
    public UnityAction<TrickManager> OnTrickFail;
    public UnityAction<float> OnTrickRunning;
    public UnityAction<TrickManager> OnTrickComplete;



}



