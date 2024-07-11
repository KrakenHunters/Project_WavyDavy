using UnityEngine;

public class TransitionState : BaseState
{
    private Vector3 targetPos;
    private bool correctPos;
    public override void EnterState()
    {
        base.EnterState();

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

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        if ((targetPos - player.transform.position).sqrMagnitude > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPos, player.speed * Time.deltaTime);
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
                    //inputManager.EnablePlayerTrickState();
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
