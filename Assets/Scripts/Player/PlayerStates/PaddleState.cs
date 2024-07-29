using UnityEngine;

public class PaddleState : BaseState
{
    private Paddle currentPaddleDir;
    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerPaddle(); // Start in first phase paddle
        currentPaddleDir = Paddle.None;
    }

    public override void ExitState()
    {
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
    }

    public override void StateUpdate()
    {
    }

    public override void HandlePaddling(Paddle paddleDir)
    {
        if (currentPaddleDir != paddleDir)
        {
            currentPaddleDir = paddleDir;
            player.Event.OnIncreaseFlow.Invoke(player.finalPaddleSpeed);
            //Player paddle animation giver the currentPaddleDir
        }
    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }
}