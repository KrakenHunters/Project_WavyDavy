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
    public UnityEvent<float> OnFlowChange;

    //public UnityEvent<GameObject> OnLoseFlow;
    //public UnityEvent<float> OnGainFlow;

    public UnityEvent OnStartTrick;
    public UnityEvent<bool> OnEndTrick;
    public UnityAction<bool> OnIsTrickPossible;

    public UnityAction OnCelebration;

    public UnityEvent<GameObject> OnGameEnd;

    public UnityEvent OnUIUpdate;

    public UnityAction<float> OnSetGameTimer;
    public UnityAction<float> OnUpdateGameTimer;

    public UnityAction<TrickCombo> OnPlayerInput;


    public UnityAction<List<TrickSO>> OnTrickInput;
    public UnityAction<PlayerTrickHandler> OnTrickStart;
    public UnityAction<float> OnTrickRunning;
    public UnityAction OnTrickHalfTime;
    public UnityAction<PlayerTrickHandler> OnTrickFinish;


    public UnityAction<PlayerTrickHandler> InitializeTrickUI;

    public UnityAction<PlayerTrickHandler> OnTrickCelebration;

   
}



