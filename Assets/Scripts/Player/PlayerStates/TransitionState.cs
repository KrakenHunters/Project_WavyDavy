using UnityEngine;

public class TransitionState : BaseState
{
    private Vector3 targetPos;

    private float inclineVelocity = 0f;

    public override void EnterState()
    {
        base.EnterState();

        player.normalSpeed = 3f;

        switch (player.currentGamePhase)
        {
            case GamePhase.Phase1:
                inputManager.DisableAllInput();
                targetPos = player.phase1StartPos.position;
                break;
            case GamePhase.Phase2:
                targetPos = player.phase2StartPos.position;
                break;
            case GamePhase.Phase3:
                targetPos = player.phase3StartPos.position;
                //inputManager.EnablePlayerTrickState();
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

        float targetInclineAngle = 0f;
        float currentInclineAngle = Mathf.SmoothDamp(player.transform.rotation.z, targetInclineAngle, ref inclineVelocity, 0.5f);
        player.transform.rotation = Quaternion.Euler(0f, 0f, currentInclineAngle);

        if ((targetPos - player.transform.position).sqrMagnitude > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, player.normalSpeed * Time.fixedDeltaTime);
        }
        else
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

    public override void StateUpdate()
    {


    }

    public override void HandleTransition()
    {

    }


}
