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

        _action.PlayerMovement.Movement.performed += (val) => HandleMovement(val.ReadValue<Vector2>());

        _action.PlayerMovement.Enable();
    }

    private void HandleMovement(Vector2 input)
    {
        _movement = input;
    }

    private void DisablePlayerMovement()
    {
        _action.PlayerMovement.Movement.performed -= (val) => _movement = val.ReadValue<Vector2>();
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

        _action.PlayerPaddle.PaddleRight.performed += (val) => _player.HandlePaddleRight(true);
        _action.PlayerPaddle.PaddleLeft.performed += (val) => _player.HandlePaddleLeft(true);

        _action.PlayerPaddle.PaddleRight.canceled += (val) => _player.HandlePaddleRight(false);
        _action.PlayerPaddle.PaddleLeft.canceled += (val) => _player.HandlePaddleLeft(false);

        _action.PlayerPaddle.Enable();
    }

    private void DisablePlayerPaddle()
    {
        _action.PlayerPaddle.PaddleRight.performed -= (val) => _player.HandlePaddleRight(true);
        _action.PlayerPaddle.PaddleLeft.performed -= (val) => _player.HandlePaddleLeft(true);

        _action.PlayerPaddle.PaddleRight.canceled -= (val) => _player.HandlePaddleRight(false);
        _action.PlayerPaddle.PaddleLeft.canceled -= (val) => _player.HandlePaddleLeft(false);


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
