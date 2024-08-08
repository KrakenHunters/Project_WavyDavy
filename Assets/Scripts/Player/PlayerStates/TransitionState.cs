using DG.Tweening;
using UnityEngine;

public class TransitionState : BaseState
{
    private Vector3 targetPos;
    float targetInclineAngle;
    public override void EnterState()
    {
        base.EnterState();

        switch (player.currentGamePhase)
        {
            case GamePhase.Phase1:
                inputManager.DisableAllInput();
                targetPos = player.phase1StartPos.position;
                player.normalSpeed = 3f;
                break;
            case GamePhase.Phase2:
                targetPos = player.phase2StartPos.position;
                player.normalSpeed = 3f;
                break;
            case GamePhase.Phase3:
                targetPos = player.phase3StartPos.position;
                player.normalSpeed = 5f;

                break;
            case GamePhase.Trick:
                targetPos = player.trickStartPos.position;
                player.normalSpeed = 7f;

                break;
        }



    }
    public override void ExitState()
    {
        player.Event.OnFinishTransition.Invoke();
    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();

        // Calculate the direction of movement
        Vector3 direction = targetPos - player.transform.position;

        // Move the player
        if (direction.sqrMagnitude > 0.01f)
        {

            // Determine the target incline angle based on the direction
            if (direction.y > 0) // Moving up
            {
                player.transform.DORotate(new Vector3(0f, 0f, player.maxInclineAngle), 0.5f); // Positive angle for moving up
            }
            else if (direction.y < 0) // Moving down
            {
                player.transform.DORotate(new Vector3(0f, 0f, -player.maxInclineAngle), 0.5f); // Negative angle for moving down
            }


            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, player.normalSpeed * Time.deltaTime);
        }
        else
        {
            player.transform.DORotate(Vector3.zero, 0.2f); // Return to 0 rotation

            // Ensure the rotation has reached the target angle before switching states
            if (Mathf.Abs(Mathf.DeltaAngle(player.transform.eulerAngles.z, 0f)) < 0.1f)
            {
                switch (player.currentGamePhase)
                {
                    case GamePhase.Phase1:
                        player.ChangeState(new PaddleState());
                        break;
                    case GamePhase.Phase2:
                        player.ChangeState(new MovementState());
                        break;
                    case GamePhase.Phase3:
                        player.ChangeState(new MovementState());
                        break;
                    case GamePhase.Trick:
                        player.ChangeState(new TrickState());
                        break;
                }
            }
        }


    }
    public override void StateUpdate()
    {
    }

    public override void HandleTransition()
    {

    }


}
