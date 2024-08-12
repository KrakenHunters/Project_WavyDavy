using UnityEngine;

public abstract class BaseState
{
    protected static readonly int TrickStart = Animator.StringToHash("TrickStart");
    protected static readonly int TrickPeak = Animator.StringToHash("TrickPeak");
    protected static readonly int TrickEnd = Animator.StringToHash("TrickEnd");

    protected static readonly int PaddleIdle = Animator.StringToHash("PaddleIdle");
    protected static readonly int PaddleLeft = Animator.StringToHash("PaddleLeft");
    protected static readonly int PaddleRight = Animator.StringToHash("PaddleRight");
    protected static readonly int PaddleGetUp = Animator.StringToHash("GetUp");
    protected static readonly int PaddleHopOn = Animator.StringToHash("HopOnBoard");


    protected static readonly int Pump = Animator.StringToHash("PumpingDown");

    protected static readonly int GetHit = Animator.StringToHash("GetHit");

    protected static readonly int Idle = Animator.StringToHash("SurfIdle");
    protected static readonly int Crash = Animator.StringToHash("Crash");


    protected static readonly int TrickFailStart = Animator.StringToHash("TrickFailStart");
    protected static readonly int TrickFailPeak = Animator.StringToHash("TrickFailPeak");
    protected static readonly int TrickFailEnd = Animator.StringToHash("TrickFailEnd");

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