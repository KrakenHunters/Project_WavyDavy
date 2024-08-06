using System.Collections.Generic;
using UnityEngine;

public class CelebrationState : BaseState
{
    protected static readonly int TrickStart = Animator.StringToHash("TrickStart");
    protected static readonly int TrickPeak = Animator.StringToHash("TrickPeak");
    protected static readonly int TrickEnd = Animator.StringToHash("TrickEnd");

    protected static readonly int TrickFailStart = Animator.StringToHash("TrickFailStart");
    protected static readonly int TrickFailPeak = Animator.StringToHash("TrickFailPeak");
    protected static readonly int TrickFailEnd = Animator.StringToHash("TrickFailEnd");

    bool celebrationInvoked;
    public override void EnterState()
    {
        celebrationInvoked = false;

        inputManager.DisableAllInput();

        if (trickManager.CurrentResult == Tricks.TrickResult.Complete)
        {
            player.animator.runtimeAnimatorController = trickManager.CurrentTrick.animationController;
            player.animator.Play(TrickStart);
        }
        else if (trickManager.CurrentResult == Tricks.TrickResult.Failed)
        {
            player.animator.Play(TrickFailStart);
        }

    }
    public override void ExitState()
    {
        celebrationInvoked = false;
    }

    public override void StateUpdate()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.GetHashCode() == TrickPeak || stateInfo.GetHashCode() == TrickFailPeak) && !celebrationInvoked)
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                celebrationInvoked = true;
                player.Event.OnTrickCelebration(trickManager);
            }
        }
    }

    public override void FinishCelebration()
    {
        if (trickManager.CurrentResult == Tricks.TrickResult.Complete)
        {
            player.animator.Play(TrickEnd);
        }
        else if (trickManager.CurrentResult == Tricks.TrickResult.Failed)
        {
            player.animator.Play(TrickFailStart);
        }
        player.Event.OnChangeGameState.Invoke(GamePhase.Phase3);
    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }


}
