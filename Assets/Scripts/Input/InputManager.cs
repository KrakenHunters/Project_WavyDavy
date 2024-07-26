using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

   PlayerInput _action;
   PlayerController _player;

    private float _holdTimeA = 0f;
    private float _holdTimeD = 0f;
    [SerializeField] private float _maxHoldTime = 2f; // Maximum hold time for full input


    Vector2 _movement;

    private bool _isKeyboardInput = false;
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
        if (_isKeyboardInput)
        {
            // Update hold times based on the current direction
            if (_movement.x > 0)
            {
                _holdTimeD += Time.deltaTime;
                _holdTimeA = 0f;
            }
            else if (_movement.x < 0)
            {
                _holdTimeA += Time.deltaTime;
                _holdTimeD = 0f;
            }
            else
            {
                _holdTimeA = 0f;
                _holdTimeD = 0f;
            }

            // Cap hold times at maximum
            _holdTimeA = Mathf.Min(_holdTimeA, _maxHoldTime);
            _holdTimeD = Mathf.Min(_holdTimeD, _maxHoldTime);

            // Scale input based on hold times
            if (_movement.x > 0)
            {
                _movement.x = Mathf.Lerp(0, 1, _holdTimeD / _maxHoldTime);
            }
            else if (_movement.x < 0)
            {
                _movement.x = Mathf.Lerp(0, -1, _holdTimeA / _maxHoldTime);
            }
            else
            {
                _movement.x = 0;
            }
        }
    }

    public void EnablePlayerMovement()
    {
        DisablePlayerPaddle();
        DisablePlayerTrickState();

        _action.PlayerMovement.Pump.performed += (val) => _player.HandlePump();
        _action.PlayerMovement.Pump.canceled += (val) => _player.HandleStopPump();

        _action.PlayerMovement.Movement.performed += (val) => HandleMovement(val.ReadValue<Vector2>());

        _action.PlayerMovement.Enable();
    }

    private void HandleMovement(Vector2 input)
    {
/*        // Detect if the input is coming from the keyboard
        if (Mathf.Approximately(input.x, 1f) || Mathf.Approximately(input.x, -1f))
        {
            _isKeyboardInput = true;
        }
        else
        {
            _isKeyboardInput = false;
        }
*/
        if (!_isKeyboardInput)
        {
            // Directly use the input for non-keyboard input (e.g., controller)
            _movement = input;
        }

    }

    private void DisablePlayerMovement()
    {
        _action.PlayerMovement.Pump.performed -= (val) => _player.HandlePump();
        _action.PlayerMovement.Pump.canceled -= (val) => _player.HandleStopPump();

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
