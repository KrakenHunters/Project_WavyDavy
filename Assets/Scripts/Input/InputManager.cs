using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   PlayerInput _action;
   //PlayerController _player;

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
        //_player = GetComponent<PlayerController>();
        _action = new PlayerInput();
    }

    private void Update()
    {

    }

    public void EnablePlayerMovement()
    {
        DisablePlayerPaddle();
        DisablePlayerTrickState();

        _action.PlayerMovement.Movement.performed += (val) => _movement = val.ReadValue<Vector2>();
        _action.PlayerMovement.Enable();
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
        _action.PlayerTrickState.Up.performed += (val) => Debug.Log("Up");
        _action.PlayerTrickState.Down.performed += (val) => Debug.Log("Down");
        _action.PlayerTrickState.Left.performed += (val) => Debug.Log("Left");
        _action.PlayerTrickState.Right.performed += (val) => Debug.Log("Right");

        _action.PlayerTrickState.Enable();
    }

    private void DisablePlayerTrickState()
    {
        _action.PlayerTrickState.Up.performed -= (val) => Debug.Log("Up");
        _action.PlayerTrickState.Down.performed -= (val) => Debug.Log("Down");
        _action.PlayerTrickState.Left.performed -= (val) => Debug.Log("Left");
        _action.PlayerTrickState.Right.performed -= (val) => Debug.Log("Right");

        _action.PlayerTrickState.Disable();
    }

    public void EnablePlayerPaddle()
    {
        DisablePlayerMovement();
        DisablePlayerTrickState();

        _action.PlayerPaddle.Paddle.performed += (val) => Debug.Log("Paddle");

        _action.PlayerPaddle.Enable();
    }

    private void DisablePlayerPaddle()
    {
        _action.PlayerPaddle.Paddle.performed -= (val) => Debug.Log("Paddle");

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
