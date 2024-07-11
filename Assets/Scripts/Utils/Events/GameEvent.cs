using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{

    public UnityEvent<GamePhase> OnChangeGameState;
    public UnityEvent<GameObject> OnFlowBarAbove;
    public UnityEvent<GameObject> OnFlowBarBelow;

    public UnityEvent<GameObject> OnHit;     

/*    public UnityEvent<GameObject> OnLoseFlow;
    public UnityEvent<GameObject> OnGainFlow;
*/
    public UnityEvent OnStartTrick;
    public UnityEvent<bool> OnEndTrick;

    public UnityEvent<GameObject> OnCelebration;

    public UnityEvent<GameObject> OnGameEnd;

    public UnityEvent OnUIUpdate;

}



