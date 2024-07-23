using UnityEngine;

public class MovementState : BaseState
{
    private float targetInclineAngle = 0f;
    private float currentInclineAngle = 0f;
    private float inclineVelocity = 0f;

    private float targetSpeed = 0f;
    private float currentSpeed = 0f;
    private float speedVelocity = 0f;

    private float upSpeedSmoothTime = 0.5f; // Smooth time for up speed transition
    private float downSpeedSmoothTime = 0.5f; // Smooth time for down speed transition

    private float upToDownSmoothTime = 0.5f; // Smooth time for rotation transitioning from up to down
    private float downToUpSmoothTime = 0.3f; // Smooth time for rotation transitioning from down to up

    private bool isMovingUp = false;
    private bool isMovingDown = false;

    private float inputCooldownTime = 0.15f; // Time between inputs in seconds
    private float lastInputTime = 0f;

    private bool queuedMoveUp = false;
    private bool queuedMoveDown = false;

    private float maxHeight;

    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerMovement();


        switch (player.currentGamePhase)
        {
            case GamePhase.Phase2:
                maxHeight = player.phase2MaxHeight;
                break;
            case GamePhase.Phase3:
                maxHeight = player.phase3MaxHeight;
                break;
        }

    }

    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();

        // Apply queued inputs after cooldown
        if (Time.time - lastInputTime >= inputCooldownTime)
        {
            if (queuedMoveUp)
            {
                isMovingUp = true;
                isMovingDown = false;
                queuedMoveUp = false;
                lastInputTime = Time.time;
            }
            else if (queuedMoveDown)
            {
                isMovingUp = false;
                isMovingDown = true;
                queuedMoveDown = false;
                lastInputTime = Time.time;
            }
        }

        float currentSmoothTime = 0.5f;
        float speedSmoothTime = 1f;

        // Update target inclination angle based on movement direction
        if (isMovingUp)
        {
            targetInclineAngle = player.maxInclineAngle;
            targetSpeed = player.speed;
            speedSmoothTime = upSpeedSmoothTime;
            currentSmoothTime = downToUpSmoothTime;
        }
        else if (isMovingDown)
        {
            targetInclineAngle = -player.maxInclineAngle;
            targetSpeed = player.speed;
            speedSmoothTime = downSpeedSmoothTime;
            currentSmoothTime = upToDownSmoothTime;
        }
        else
        {
            targetInclineAngle = 0f;
            targetSpeed = 0f;
        }

        // Smoothly transition to the target inclination angle
        currentInclineAngle = Mathf.SmoothDamp(currentInclineAngle, targetInclineAngle, ref inclineVelocity, currentSmoothTime);

        // Smoothly transition to the target speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, speedSmoothTime);
        // Apply vertical movement
        Vector3 verticalMovement = (isMovingUp ? Vector3.up : (isMovingDown ? Vector3.down : Vector3.zero)) * currentSpeed * Time.deltaTime;

        // Check if player is exceeding maxHeight
        if (player.transform.position.y + verticalMovement.y > maxHeight)
        {
            // Limit vertical movement to maxHeight
            verticalMovement.y = maxHeight - player.transform.position.y;
        }

        player.transform.position += verticalMovement;

        // Apply inclination
        player.transform.rotation = Quaternion.Euler(0f, 0f, currentInclineAngle);
    }

    public override void StateUpdate()
    {
    }

    public override void HandleMovement()
    {
        currentSpeed = currentSpeed/4f;
        queuedMoveUp = true;
        queuedMoveDown = false;
    }

    public override void HandleStopMovement()
    {
        currentSpeed = currentSpeed / 4f;
        queuedMoveDown = true;
        queuedMoveUp = false;
    }

    public override void HandleTransition()
    {
        isMovingUp = false;
        isMovingDown = false;
        base.HandleTransition();
    }
}