using UnityEngine;
//using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   /* PlayerInput _action;
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
    private void OnEnable()
    {

        _action.Player.Interact.performed += (val) => _player.HandleInteract();
        _action.Player.Interact.canceled += (val) => _player.CancelInteract();

        _action.Enable();
    }

    private void OnDisable()
    {
        _action.Player.Interact.performed -= (val) => _player.HandleInteract();
        _action.Player.Interact.canceled -= (val) => _player.CancelInteract();



        _action.Disable();
    }*/
}
