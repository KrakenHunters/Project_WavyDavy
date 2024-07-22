using UnityEngine;

public abstract class BaseState
{
    public PlayerController player { get; set; }
    public TrickManager trickManager { get; set; }

    protected Vector3 _direction;
    protected float timer;

    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandlePaddling() { }

    public virtual void HandleTransition() 
    {
        player.ChangeState(new TransitionState());
    }

}