using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class TrickState : BaseState
{
    private CountdownTimer trickTimer;

    public override void EnterState()
    {
        inputManager.EnablePlayerTrickState();
        trickTimer = new CountdownTimer(trickManager.trickBeginTime);
        trickManager.StartTrick();
    }

    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {
        trickTimer.Tick(Time.deltaTime);
        player.Event.OnTrickRunning.Invoke(trickTimer.Progress);

        if (trickTimer.IsFinished)
        {
            trickManager.EndTrick();
            trickTimer.Reset();
            player.Event.OnTrickFail?.Invoke(trickManager);
        }

    }

    public override void HandlePaddling()
    {

    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }
}
