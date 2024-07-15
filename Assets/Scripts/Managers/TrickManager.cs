using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utilities;

public class TrickManager : Singleton<TrickManager>
{
    [SerializeField] private TrickSO[] tricks;
    [SerializeField] private float trickBeginTime;
    [SerializeField] private float trickPressTime;

    private List<TrickCombo> buttonPressed = new List<TrickCombo>();
    private TrickSO currentTrick;

    private readonly List<TrickSO> upTricks = new();
    private readonly List<TrickSO> downTricks = new();
    private readonly List<TrickSO> leftTricks = new();
    private readonly List<TrickSO> rightTricks = new();

    private bool _isTrickActive;
    private bool _isTrickComplete;
    private bool _isTrickFailed;
    private int _currentTrickIndex;

    private CountdownTimer _trickBeginTimer;
    private CountdownTimer _trickPressTimer;

    private void Start()
    {
        _trickBeginTimer = new CountdownTimer(trickBeginTime);
        _trickPressTimer = new CountdownTimer(trickPressTime);
        
        _trickBeginTimer.OnTimerStop += CheckBeginTime;
        _trickBeginTimer.OnTimerStop += () => _trickBeginTimer.Start();



        ProccessTricks();
    }

   private void CheckBeginTime()
    {

    }
    private void CheckPressTime()
    {

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
            _isTrickActive = true;
            currentTrick = trick;
            Debug.Log($" the trick is ..... {trick.trickName}");
        }

    }

    private void TrackActiveTrick(TrickSO trick)
    {
        while (_isTrickActive)
        {
            while (trick.trickCombo[_currentTrickIndex] == buttonPressed[_currentTrickIndex])
            {
                if (buttonPressed.Count > _currentTrickIndex + 1)
                {
                    _currentTrickIndex++;
                    if (_currentTrickIndex == trick.trickCombo.Count)
                    {
                        _isTrickComplete = true;
                        _isTrickActive = false;
                        currentTrick = null;
                        Debug.Log("Trick Complete");
                    }
                }
            }
            if (buttonPressed[_currentTrickIndex] != trick.trickCombo[_currentTrickIndex])
            {
                _isTrickFailed = true;
                _isTrickActive = false;
                currentTrick = null;
                Debug.Log("Trick Failed");
            }
        }
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
        _currentTrickIndex = 1;
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
