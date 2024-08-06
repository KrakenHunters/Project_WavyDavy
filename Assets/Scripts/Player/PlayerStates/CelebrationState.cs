using System.Collections.Generic;
using UnityEngine;

public class CelebrationState : BaseState
{
    protected static readonly int TrickCompleted = Animator.StringToHash("Trick");
    protected static readonly int TrickFailed = Animator.StringToHash("Trick Fail");

    private bool isAnimationPlaying;

    private AnimatorOverrideController animatorOverrideController;
    bool celebrationInvoked;
    public override void EnterState()
    {
        base.EnterState();

        isAnimationPlaying = false;
        celebrationInvoked = false;

        inputManager.DisableAllInput();

        if (trickManager.CurrentResult == Tricks.TrickResult.Complete)
        {
            player.animator.runtimeAnimatorController = trickManager.CurrentTrick.animationController;

            player.animator.Play(TrickCompleted);
            isAnimationPlaying = true;
        }
        else if (trickManager.CurrentResult == Tricks.TrickResult.Failed)
        {
            player.animator.Play(TrickFailed);
            isAnimationPlaying = true;
        }

    }
    public override void ExitState()
    {
        isAnimationPlaying = false;
        celebrationInvoked = false;
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
    }

    public override void StateUpdate()
    {
        if (isAnimationPlaying && !celebrationInvoked)
        {
            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1.0f)
            {
                player.Event.OnTrickCelebration(trickManager);
                celebrationInvoked = true;
            }
        }
    }

    private void ChangeAnimationClip(string stateName, AnimationClip newClip)
    {
        // Create a list of animation clip pairs
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(animatorOverrideController.overridesCount);

        // Populate the list with the current overrides
        animatorOverrideController.GetOverrides(overrides);

        // Find the clip you want to replace and update it
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == stateName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newClip);
                break;
            }
        }

        // Apply the overrides
        animatorOverrideController.ApplyOverrides(overrides);
    }
    public override void HandleTransition()
    {
        base.HandleTransition();
    }


}
