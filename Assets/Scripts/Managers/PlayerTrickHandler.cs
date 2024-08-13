using System.Collections.Generic;
using Tricks;
using UnityEngine;

public class PlayerTrickHandler : MonoBehaviour
{
    public GameEvent Event;

    public TrickSO[] tricks;
    public float trickBeginTime;

    public float MaxTrickTime => trickBeginTime;

    public GamePhase currentPhase = GamePhase.Phase1;
    public List<TrickSO> possibleTrickCombos = new List<TrickSO>();
    private List<TrickCombo> playerPressedCombo = new List<TrickCombo>();

    public TrickSO CurrentTrick { get; private set; }
    public TrickResult CurrentResult { get; private set; }

    [SerializeField]
    private TrickUIHandler trickUIHandler;

    [SerializeField] private AudioClip trickCompleted;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip trickFailed;

    private void OnEnable()
    {
        Event.OnPlayerInput += AddButton;
    }
    private void OnDisable()
    {
        Event.OnPlayerInput -= AddButton;
    }

    private void Awake()
    {
        trickUIHandler.Initialize(this);
    }

    public void StartTrick()
    {
        Event.OnTrickStart?.Invoke(this);
    }

    public void EndTrick()
    {

        if(CurrentResult == TrickResult.Complete)
        {
            AudioManager.Instance.PlayAudio(trickCompleted);
        }
        else if(CurrentResult == TrickResult.Failed)
        {
            AudioManager.Instance.PlayAudio(trickFailed);
        }

        playerPressedCombo.Clear();
        trickUIHandler.PlayerInputCount = playerPressedCombo.Count - 1;

        Event.OnTrickCelebration?.Invoke(this);
    }

    public void AddButton(TrickCombo move)
    {
        playerPressedCombo.Add(move);
        AudioManager.Instance.PlayAudio(clickSound);
        trickUIHandler.PlayerInputCount = playerPressedCombo.Count - 1;
        CheckMatchCombos();
        //CheckMatchingCombos();
        Event.OnTrickInput?.Invoke(this.possibleTrickCombos);
    }

    public void CheckMatchCombos()
    {
        TrickResult result = TrickResult.Default;
        possibleTrickCombos.Clear();
        foreach (TrickSO trick in tricks)
        {
            result = trick.Evaluate(playerPressedCombo);
            if (result == TrickResult.Possible)
                possibleTrickCombos.Add(trick);
            if (result == TrickResult.Complete)
            {
                CurrentResult = result;
                CurrentTrick = trick;
                Event.OnTrickFinish?.Invoke(this);
                EndTrick();
                //ADD POINTS
                Debug.Log(trick.TrickName + " is Complete");
            }
        }
        if (result == TrickResult.Failed && possibleTrickCombos.Count == 0)
        {
            TrickFailed(result);
        }
    }

    public void TrickFailed(TrickResult result)
    {
        Debug.Log("TrickFailed");
        CurrentResult = result;
        Event.OnTrickFinish?.Invoke(this);
        EndTrick();
    }

}

public enum TrickCombo
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
