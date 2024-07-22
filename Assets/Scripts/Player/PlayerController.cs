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

    public GameEvent Event;

    public GamePhase currentGamePhase{ get; private set; }

    public Transform phase1StartPos;
    public Transform phase2StartPos;
    public Transform phase3StartPos;
    public float speed;
    public float paddleSpeed;

    [HideInInspector]
    public BaseState currentState;

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

        HandleMove(inputManager.Movement);

    }

    private void FixedUpdate() => currentState?.StateFixedUpdate();

    public void HandleMove(Vector2 dir)
    {
        currentState?.HandleMovement(dir);
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
        currentState.inputManager = this.inputManager;
        currentState.EnterState();

    }

}
