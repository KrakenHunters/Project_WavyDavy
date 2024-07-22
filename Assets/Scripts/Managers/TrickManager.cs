using UnityEngine;
using System;
using System.Collections.Generic;
using Utilities;

public class TrickManager : Singleton<TrickManager>
{
    public GameEvent Event;
    public event Action OnPlayerInput;
    public event Action OnTrickStart;
    public event Action OnTrickFail;
    public event Action OnTrickComplete;

    public TrickSO[] tricks;
    [SerializeField] private float trickBeginTime;
    public float MaxTrickTime { get { return trickBeginTime; } }
    public GamePhase currentPhase = GamePhase.Phase1;
    public List<TrickSO> possibleTrickCombos = new List<TrickSO>();
    private List<TrickCombo> playerPressedCombo = new List<TrickCombo>();

    private TrickSO currentTrick;

    private bool _isTrickComplete;
    private bool _isTrickFailed;

    public CountdownTimer _trickTimer;



    private void OnEnable()
    {
        _trickTimer = new CountdownTimer(trickBeginTime);
        Event.OnChangeGameState.AddListener(CheckState);
    }
    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(CheckState);
    }

    private void CheckState(GamePhase phase)
    {
        currentPhase = phase;
        if (currentPhase == GamePhase.Trick)
        {
            StartTrick();
            currentPhase = GamePhase.Phase1;// change this later switch to a new phase ( default phase)
        }
    }

    private void StartTrick()
    {
        OnTrickStart?.Invoke();
        _trickTimer.Start();
    }

    private void EndTrick()
    {
        _trickTimer.Stop();
        
        Debug.Log("Trick Ended");
    }

    private void Update()
    {
        _trickTimer.Tick(Time.deltaTime);
        if (_trickTimer.IsFinished)
        {
            EndTrick();
           _trickTimer.Reset();
           OnTrickFail?.Invoke();
        }
        //Debug only delete later
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            AddButton(TrickCombo.UP);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            AddButton(TrickCombo.DOWN);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            AddButton(TrickCombo.LEFT);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            AddButton(TrickCombo.RIGHT);
        }
    }

    public void AddButton(TrickCombo move)
    {
        playerPressedCombo.Add(move);
        CheckMatchingCombos();
        OnPlayerInput?.Invoke();
    }

    public void CheckMatchingCombos()
    {
        possibleTrickCombos.Clear();
        bool hasMatch = false;
        foreach (TrickSO trick in tricks)
        {
            for (int i = 0; i < playerPressedCombo.Count; i++)
            {
                if (trick.trickCombo.Count < playerPressedCombo.Count)
                {
                    continue;
                }

                if (trick.trickCombo[i] == playerPressedCombo[i])
                {
                    hasMatch = true;
                }
                else
                {
                    hasMatch = false;
                    break;
                }
            }
            if (hasMatch)
            {
                possibleTrickCombos.Add(trick);
            }
        }
        // Debug.Log("Possible tricks: " + possibleTrickCombos.Count);
        if (possibleTrickCombos.Count == 1 && CheckCombosAreEqual(possibleTrickCombos[0].trickCombo, playerPressedCombo))
        {
            currentTrick = possibleTrickCombos[0];
            possibleTrickCombos.Clear();
            playerPressedCombo.Clear();
            Debug.Log("Trick: " + currentTrick.trickName + " is Complete");
            OnTrickComplete?.Invoke();
            EndTrick();
        }
        else if (possibleTrickCombos.Count == 0)
        {
            possibleTrickCombos.Clear();
            playerPressedCombo.Clear();
            Debug.Log("Combo Faild");
            OnTrickFail?.Invoke();

            EndTrick();
        }
    }

    private bool CheckCombosAreEqual(List<TrickCombo> combo1, List<TrickCombo> combo2)
    {
        if (combo1.Count != combo2.Count)
        {
            return false;
        }
        for (int i = 0; i < combo1.Count; i++)
        {
            if (combo1[i] != combo2[i])
            {
                return false;
            }
        }
        return true;
    }

}

public enum TrickCombo
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
