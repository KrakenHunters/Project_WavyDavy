using UnityEngine;

public class PaddleState : BaseState
{
    bool paddleLeft;
    bool paddleRight;

    private Paddle lastPaddleDir;
    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerPaddle(); // Start in first phase paddle
        lastPaddleDir = Paddle.None;
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
        timer += Time.deltaTime;
        GoPaddle();

    }

    public override void HandlePaddlingRight(bool isPaddling)
    {
        paddleRight = isPaddling;
        timer = 0f;
    }

    public override void HandlePaddlingLeft(bool isPaddling)
    {
        paddleLeft = isPaddling;
        timer = 0f;
    }


    private void GoPaddle()
    {
        if (timer >= 0.05f)
        {
            if (paddleRight && !paddleLeft && lastPaddleDir != Paddle.Right)
            {
                lastPaddleDir = Paddle.Right;
                player.Event.OnIncreaseFlow.Invoke(player.finalPaddleSpeed);

            }
            else if (!paddleRight && paddleLeft && lastPaddleDir != Paddle.Left)
            {
                lastPaddleDir = Paddle.Left;
                player.Event.OnIncreaseFlow.Invoke(player.finalPaddleSpeed);
            }
        }
    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }
}