using UnityEngine;
using System.Collections.Generic;
using Utilities;

public class TrickManager : Singleton<TrickManager>
{
    [SerializeField] private TrickSO[] tricks;
    [SerializeField] private float trickBeginTime;

    private List<TrickCombo> playerPressedCombo = new List<TrickCombo>();
    private List<TrickSO> possibleTrickCombos = new List<TrickSO>();

    private TrickSO currentTrick;

    private bool _isTrickActive;
    private bool _isTrickComplete;
    private bool _isTrickFailed;

    private CountdownTimer _trickBeginTimer;

    private CountdownTimer _trickBeginTimer;
    private CountdownTimer _trickPressTimer;

    private void Start()
    {
        _trickBeginTimer = new CountdownTimer(trickBeginTime);
    }

    public void AddButton(TrickCombo move)
    {
        playerPressedCombo.Add(move);
        CheckMatchingCombos();
    }


    private void Update()
    {

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

    public void CheckMatchingCombos()
    {
        possibleTrickCombos.Clear();
        bool hasMatch = false;  
        foreach (TrickSO trick in tricks)
        {
            for(int i = 0; i < playerPressedCombo.Count; i++)
            {
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
                Debug.Log("Possible tricks: " + possibleTrickCombos.Count);
        if (possibleTrickCombos.Count == 1 && CheckCombosAreEqual(possibleTrickCombos[0].trickCombo , playerPressedCombo))
        {
            currentTrick = possibleTrickCombos[0];
            _isTrickActive = true;
            possibleTrickCombos.Clear();
            playerPressedCombo.Clear();
            Debug.Log("Trick: " + currentTrick.trickName + " is Complete");
        } else if (possibleTrickCombos.Count == 0)
        {
            possibleTrickCombos.Clear();
            playerPressedCombo.Clear();
            Debug.Log("Combo Faild");
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
