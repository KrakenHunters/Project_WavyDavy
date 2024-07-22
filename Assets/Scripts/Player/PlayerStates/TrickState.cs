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
        //trickTimer = new CountdownTimer(trickBeginTime);
    }

    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {

    }

    public override void StateUpdate()
    {

    }

    public override void HandlePaddling()
    {
    }

    public override void HandleTransition()
    {
    }
}
