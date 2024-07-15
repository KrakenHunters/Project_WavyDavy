using System.Collections.Generic;
using UnityEngine;

public class TrickManager : Singleton<TrickManager>
{
    [SerializeField] private TrickSO[] tricks;

    private List<TrickCombo> buttonPressed = new List<TrickCombo>();
    private TrickSO currentTrick;

    private readonly List<TrickSO> upTricks = new();
    private readonly List<TrickSO> downTricks = new();
    private readonly List<TrickSO> leftTricks = new();
    private readonly List<TrickSO> rightTricks = new();

    private bool isTrickActive;
    private bool isTrickComplete;
    private bool isTrickFailed;
    private int currentTrickIndex;

    private void Start()
    {
        ProccessTricks();
    }

    public void ProccessTricks()
    {
        foreach (TrickSO trick in tricks)
        {
            switch (trick.trickCombo[0])
            {
                case TrickCombo.UP:
                    upTricks.Add(trick);
                    break;
                case TrickCombo.DOWN:
                    downTricks.Add(trick);
                    break;
                case TrickCombo.LEFT:
                    leftTricks.Add(trick);
                    break;
                case TrickCombo.RIGHT:
                    rightTricks.Add(trick);
                    break;
                default:
                    Debug.LogWarning($"Missing trick in SO {trick}");
                    break;
            }
        }
    }

    public void AddButton(TrickCombo move)
    {
        buttonPressed.Add(move);
        CheckCombo();
    }

    private void Update()
    {
        // Debug only: delete later
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

        if (isTrickActive)
        {
            TrackActiveTrick();
        }
    }

    public void CheckCombo()
    {
        List<TrickSO> trickGroup = FindTrickGroup();
        if (buttonPressed.Count > 1)
        {
            TrickSO trick = FindTrickCombo(trickGroup);
            if (trick == null)
            {
                buttonPressed.RemoveAt(0);
                return;
            }
            isTrickActive = true;
            currentTrick = trick;
            currentTrickIndex = 1;
            Debug.Log($"The trick is: {trick.trickName}");
        }
    }

    private void TrackActiveTrick()
    {
        while (isTrickActive)
        {
            if (currentTrickIndex >= currentTrick.trickCombo.Count)
            {
                CompleteTrick();
                return;
            }

            if (buttonPressed.Count <= currentTrickIndex || buttonPressed[currentTrickIndex] != currentTrick.trickCombo[currentTrickIndex])
            {
                FailTrick();
                return;
            }

            currentTrickIndex++;

            if (currentTrickIndex == currentTrick.trickCombo.Count)
            {
                CompleteTrick();
            }
        }
    }

    private void CompleteTrick()
    {
        isTrickComplete = true;
        isTrickActive = false;
        Debug.Log("Trick Complete");
        ResetTrick();
    }

    private void FailTrick()
    {
        isTrickFailed = true;
        isTrickActive = false;
        Debug.Log("Trick Failed");
        ResetTrick();
    }

    private void ResetTrick()
    {
        currentTrick = null;
        buttonPressed.Clear();
        currentTrickIndex = 0;
        isTrickComplete = false;
        isTrickFailed = false;
    }

    private List<TrickSO> FindTrickGroup()
    {
        switch (buttonPressed[0])
        {
            case TrickCombo.UP:
                return upTricks;
            case TrickCombo.DOWN:
                return downTricks;
            case TrickCombo.LEFT:
                return leftTricks;
            case TrickCombo.RIGHT:
                return rightTricks;
            default:
                return null;
        }
    }

    private TrickSO FindTrickCombo(List<TrickSO> trickGroup)
    {
        foreach (TrickSO trick in trickGroup)
        {
            if (buttonPressed[1] == trick.trickCombo[1])
            {
                return trick;
            }
        }
        return null;
    }
}

public enum TrickCombo
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
