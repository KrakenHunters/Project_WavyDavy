using UnityEngine;

public abstract class BaseState
{
    public PlayerController player { get; set; }
    public PlayerTrickHandler trickManager { get; set; }

    protected Vector3 _direction;
    protected float timer;

    public InputManager inputManager { get; set; }

    protected BaseState currentState;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateUpdate() { }
    public virtual void HandleMovement(Vector2 dir) { }
    public virtual void HandlePumping() { }

    public virtual void HandleCelebration() { }

    public virtual void HandleStopPumping() { }
    public virtual void HandlePaddlingRight(bool isPaddling) { }
    public virtual void HandlePaddlingLeft(bool isPaddling) { }
    public virtual void HitObject() { }

    public virtual void HandleTransition() 
    {
        player.ChangeState(new TransitionState());
    }

    public virtual void FinishCelebration() { }


}
public enum Paddle
{
    None,
    Right,
    Left
}