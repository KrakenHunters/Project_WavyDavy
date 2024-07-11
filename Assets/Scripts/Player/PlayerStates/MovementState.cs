using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovementState : BaseState
{
    public override void EnterState()
    {

        base.EnterState();
        inputManager.EnablePlayerMovement(); 

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        player.transform.position = player.transform.position + _direction * player.speed * Time.deltaTime;

    }

    public override void StateUpdate()
    {
    }

    public override void HandleMovement(Vector2 direction)
    {
        Debug.Log("Moving");
        _direction = direction;
    }

    public override void HandleTransition()
    {
        base.HandleTransition();
    }
}
