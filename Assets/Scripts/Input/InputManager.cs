using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

   PlayerInput _action;
   PlayerController _player;


    Vector2 _movement;
    public Vector2 Movement
    {
        get
        {
            return _movement;
        }
        private set
        {
            _movement = value;
        }
    }
    void Awake()
    {
        _player = GetComponent<PlayerController>();
        _action = new PlayerInput();
    }

    private void Update()
    {

    }

    public void EnablePlayerMovement()
    {
        DisablePlayerPaddle();
        DisablePlayerTrickState();

        _action.PlayerMovement.Movement.performed += (val) => _player.HandleMove();
        _action.PlayerMovement.Movement.canceled += (val) => _player.HandleStopMove();

        _action.PlayerMovement.Enable();
    }

    private void DisablePlayerMovement()
    {
        _action.PlayerMovement.Movement.performed -= (val) => _player.HandleMove();
        _action.PlayerMovement.Movement.canceled -= (val) => _player.HandleStopMove();
        _action.PlayerMovement.Disable();
    }

    public void EnablePlayerTrickState()
    {
        DisablePlayerPaddle();
        DisablePlayerMovement();
        _action.PlayerTrickState.Up.performed += (val) =>   _player.HandleTrickInput(TrickCombo.UP);
        _action.PlayerTrickState.Down.performed += (val) => _player.HandleTrickInput(TrickCombo.DOWN);
        _action.PlayerTrickState.Left.performed += (val) => _player.HandleTrickInput(TrickCombo.LEFT);
        _action.PlayerTrickState.Right.performed += (val) => _player.HandleTrickInput(TrickCombo.RIGHT);

        _action.PlayerTrickState.Enable();
    }


    private void DisablePlayerTrickState()
    {
        _action.PlayerTrickState.Up.performed -= (val) =>   _player.HandleTrickInput(TrickCombo.UP);
        _action.PlayerTrickState.Down.performed -= (val) => _player.HandleTrickInput(TrickCombo.DOWN);
        _action.PlayerTrickState.Left.performed -= (val) => _player.HandleTrickInput(TrickCombo.LEFT);
        _action.PlayerTrickState.Right.performed -= (val) => _player.HandleTrickInput(TrickCombo.RIGHT);

        _action.PlayerTrickState.Disable();
    }

    public void EnablePlayerPaddle()
    {
        DisablePlayerMovement();
        DisablePlayerTrickState();

        _action.PlayerPaddle.Paddle.performed += (val) => _player.HandlePaddle();

        _action.PlayerPaddle.Enable();
    }

    private void DisablePlayerPaddle()
    {
        _action.PlayerPaddle.Paddle.performed -= (val) => _player.HandlePaddle();

        _action.PlayerPaddle.Disable();
    }

    public void DisableAllInput()
    {
        DisablePlayerMovement();
        DisablePlayerPaddle();
        DisablePlayerTrickState();
    }

    private void OnEnable()
    {
        _action.Enable();
    }
    private void OnDisable()
    {
        _action.Disable();
    }
}
