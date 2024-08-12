using System.Collections.Generic;
using UnityEngine;

public class CelebrationState : BaseState
{

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
        player.animator.Play(Idle);

        AudioManager.Instance.PlayAudio(player.scpart2Clip);
        AudioManager.Instance.SlowDownBGMusic(1f);

    }

    public override void StateUpdate()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.shortNameHash == TrickEnd || stateInfo.shortNameHash == TrickFailEnd) && !celebrationInvoked)
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                Debug.Log("End");
                celebrationInvoked = true;
                player.Event.OnChangeGameState.Invoke(GamePhase.Phase3);
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
            player.animator.Play(TrickFailEnd);
        }
    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }


}
