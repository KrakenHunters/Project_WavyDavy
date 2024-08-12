using UnityEngine;

public class PaddleState : BaseState
{
    bool paddleLeft;
    bool paddleRight;

    private Paddle lastPaddleDir;
    private AnimatorStateInfo stateInfo;
    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerPaddle(); // Start in first phase paddle
        lastPaddleDir = Paddle.None;
        player.animator.CrossFade(PaddleHopOn, 0.2f);

    }

    public override void ExitState()
    {
        player.animator.speed = 1f;
        player.animator.CrossFade(PaddleGetUp, 0.2f);
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
    }

    public override void StateUpdate()
    {
        stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
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

        float multiplier = 1f;

        if (paddleRight && !paddleLeft && lastPaddleDir != Paddle.Right)
        {

            if (stateInfo.shortNameHash == PaddleLeft)
            {
                if (stateInfo.normalizedTime <= 0.8f)
                {
                    multiplier = 0f;
                }
                else
                {
                    multiplier = Mathf.Clamp(stateInfo.normalizedTime, 0f, 1f);

                }
            }

            player.Event.OnIncreaseFlow.Invoke(player.finalPaddleSpeed * multiplier);

            lastPaddleDir = Paddle.Right;
            player.animator.speed *= (1 + player.currentFlow + 0.05f);

            player.Event.OnPaddleRight.Invoke();
            AudioManager.Instance.PlayAudio(player.paddleClip);

            player.animator.CrossFade(PaddleRight, 0.1f);

        }
        else if (!paddleRight && paddleLeft && lastPaddleDir != Paddle.Left)
        {
            if (stateInfo.shortNameHash == PaddleRight)
            {
                if (stateInfo.normalizedTime <= 0.8f)
                {
                    multiplier = 0f;
                }
                else
                {
                    multiplier = Mathf.Clamp(stateInfo.normalizedTime, 0f, 1f);
                }
            }

            player.Event.OnIncreaseFlow.Invoke(player.finalPaddleSpeed * multiplier);

            lastPaddleDir = Paddle.Left;

            if (player.animator.speed <= 2f)
                player.animator.speed *= (1 + player.currentFlow + 0.05f);

            player.Event.OnPaddleLeft.Invoke();
            AudioManager.Instance.PlayAudio(player.paddleClip);

            player.animator.CrossFade(PaddleLeft, 0.1f);

        }

    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }
}