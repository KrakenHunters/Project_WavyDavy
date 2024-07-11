using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(InputManager),typeof(PlayerHitHandler))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    private float speed;

    public Vector2 MovementDirection { get; private set; }
    public GameEvent Event;


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
        switch (gameState)
        {
            case GamePhase.Phase1:
                inputManager.EnablePlayerPaddle();
                break;
            case GamePhase.Phase2:
                inputManager.EnablePlayerMovement();
                break;
            case GamePhase.Phase3:
                //inputManager.EnablePlayerTrickState();
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.EnablePlayerMovement(); //Move this
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
        }

        Debug.Log(inputManager.Movement);

        HandleCharacterMovement();
    }

    //Move this to movement state!
    void HandleCharacterMovement() 
    {
        MovementDirection = inputManager.Movement.normalized;
        transform.position = transform.position + (Vector3)MovementDirection * speed * Time.deltaTime;

    }




}
