using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class TrickState : BaseState
{
    private CountdownTimer trickTimer;
    private bool calledHalfTime;
    public override void EnterState()
    {
        inputManager.EnablePlayerTrickState();
        trickTimer = new CountdownTimer(trickManager.trickBeginTime);
        trickTimer.Start();
        trickManager.StartTrick();
        calledHalfTime = false;

        AudioManager.Instance.PlayAudio(player.scpart1Clip);
        AudioManager.Instance.SlowDownBGMusic(0.5f);

    }

    public override void ExitState()
    {
        trickTimer.Stop();

    }

    public override void StateUpdate()
    {
        trickTimer.Tick(Time.deltaTime);
        player.Event.OnTrickRunning.Invoke(trickTimer.Progress);
        if (trickTimer.Progress < 0.5f && !calledHalfTime)
        {
            player.Event.OnTrickHalfTime?.Invoke();
            calledHalfTime = true;
        }

        if (trickTimer.IsFinished)
        {
            trickManager.TrickFailed(Tricks.TrickResult.Failed);
            Debug.Log("Trick Failed");
        }

    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }

    public override void HandleCelebration()
    {
        player.ChangeState(new CelebrationState());
    }


}
