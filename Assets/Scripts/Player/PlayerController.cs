using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Rendering.ShadowCascadeGUI;

[RequireComponent(typeof(InputManager),typeof(PlayerHitHandler))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private TrickManager trickManager;

    public GameEvent Event;

    public GamePhase currentGamePhase{ get; private set; }
    public BaseState currentState { get; private set; }


    [SerializeField] private Transform _phase1StartPos;
    [SerializeField] private Transform _phase2StartPos;
    [SerializeField] private Transform _phase3StartPos;
    [SerializeField] private float _speed;
    [SerializeField] private float _paddleSpeed;
    [SerializeField] private float _phase2MaxHeight;
    [SerializeField] private float _phase3MaxHeight;
    [SerializeField] private float _maxInclineAngle = 20f; // Maximum angle for inclination
    [SerializeField] private float _inclineSpeed = 45f; // Speed of inclination change

    public Transform phase1StartPos { get => _phase1StartPos; private set => _phase1StartPos = value; }
    public Transform phase2StartPos { get => _phase2StartPos; private set => _phase2StartPos = value; }
    public Transform phase3StartPos { get => _phase3StartPos; private set => _phase3StartPos = value; }
    public float speed { get => _speed; private set => _speed = value; }
    public float paddleSpeed { get => _paddleSpeed; private set => _paddleSpeed = value; }
    public float phase2MaxHeight { get => _phase2MaxHeight; private set => _phase2MaxHeight = value; }
    public float phase3MaxHeight { get => _phase3MaxHeight; private set => _phase3MaxHeight = value; }
    public float maxInclineAngle { get => _maxInclineAngle; private set => _maxInclineAngle = value; }
    public float inclineSpeed { get => _inclineSpeed; private set => _inclineSpeed = value; }


    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
    }

    private void SetPhase(GamePhase gameState)
    {
        currentGamePhase = gameState;
        currentState?.HandleTransition();

    }

    public void ChangeState(BaseState newState)
    {
        StartCoroutine(WaitFixedFrame(newState));
    }
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        trickManager = GetComponent<TrickManager>();
        ChangeState(new PaddleState());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputManager.EnablePlayerPaddle();
            GameManager.Instance.Event.OnChangeGameState.Invoke(GamePhase.Phase1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputManager.EnablePlayerMovement();
            GameManager.Instance.Event.OnChangeGameState.Invoke(GamePhase.Phase2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputManager.EnablePlayerMovement();
            GameManager.Instance.Event.OnChangeGameState.Invoke(GamePhase.Phase3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inputManager.EnablePlayerTrickState();
            GameManager.Instance.Event.OnChangeGameState.Invoke(GamePhase.Trick);
        }

        currentState?.StateUpdate();
    }

    private void FixedUpdate() => currentState?.StateFixedUpdate();

    public void HandleMove()
    {
        currentState?.HandleMovement();
    }

    public void HandleStopMove()
    {
        currentState?.HandleStopMovement();
    }


    public void HandlePaddle()
    {
        currentState?.HandlePaddling();
    }

    public void HandleTrickInput(TrickCombo direction)
    {
       Event.OnPlayerInput?.Invoke(direction);
    }



    private IEnumerator WaitFixedFrame(BaseState newState)
    {

        yield return new WaitForFixedUpdate();
        currentState?.ExitState();
        currentState = newState;
        currentState.player = this;
        currentState.trickManager = trickManager;
        currentState.inputManager = this.inputManager;
        currentState.EnterState();

    }

}
