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
        playerPressedCombo.Clear();
        Event.OnChangeGameState.Invoke(GamePhase.Phase3);
    }

    public void AddButton(TrickCombo move)
    {
        playerPressedCombo.Add(move);
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
            CurrentResult = result;
            Event.OnTrickFinish?.Invoke(this);
            EndTrick();
            Debug.Log("Trick Faild");
        }
    }

    /*    public void CheckMatchingCombos() //Way too long and hard codes how you set up your trick detection.
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
                if (hasMatch || playerPressedCombo.Count == 0)
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
                Event.OnTrickComplete?.Invoke(this);
                EndTrick();
            }
            else if (possibleTrickCombos.Count == 0)
            {
                possibleTrickCombos.Clear();
                playerPressedCombo.Clear();
                Debug.Log("Combo Faild");
                Event.OnTrickFail?.Invoke(this);
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
        }*/

}

public enum TrickCombo
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
