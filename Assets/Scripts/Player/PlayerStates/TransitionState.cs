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
                targetInclineAngle = 0f;
                break;
            case GamePhase.Phase2:
                targetPos = player.phase2StartPos.position;
                player.normalSpeed = 3f;
                targetInclineAngle = 0f;
                break;
            case GamePhase.Phase3:
                targetPos = player.phase3StartPos.position;
                player.normalSpeed = 5f;
                targetInclineAngle = 0f;

                break;
            case GamePhase.Trick:
                AudioManager.Instance.PlayAudio(player.scpart1Clip);
                targetPos = player.trickStartPos.position;
                player.normalSpeed = 7f;
                targetInclineAngle = player.transform.rotation.z;

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

        float currentInclineAngle = Mathf.LerpAngle(player.transform.eulerAngles.z, targetInclineAngle, Time.deltaTime * 3f);
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
