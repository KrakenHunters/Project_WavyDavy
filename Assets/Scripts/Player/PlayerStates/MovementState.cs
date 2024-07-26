using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MovementState : BaseState
{
    private float currentInclineAngle = 0f;
    private float inclineVelocity = 0f;

    private float currentSpeed = 0f;
    private float speedVelocity = 0f;

    private float upSpeedSmoothTime = 0.8f;
    private float downSpeedSmoothTime = 0.3f;

    private float upToDownSmoothTime = 0.2f;
    private float downToUpSmoothTime = 0.2f;

    private bool isPumping = false;

    private float buttonHoldTime = 0f;

    private float maxHeight;
    private float minHeight = -5f;

    private float baseReleaseForce = 0.5f;
    private float releaseForce;
    private float carveForce;
    private float timeToMaxPump;

    private float releaseForceTime = 0f;
    private float resetAngleTime = 0f;

    float targetAngle;
    float currentAngle;
    float speed;



    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerMovement();

        switch (player.currentGamePhase)
        {
            case GamePhase.Phase2:
                maxHeight = player.phase2MaxHeight;
                timeToMaxPump = 1f;
                break;
            case GamePhase.Phase3:
                maxHeight = player.phase3MaxHeight;
                timeToMaxPump = 2f;
                break;
        }
    }

    public override void ExitState() 
    { 

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();

        currentAngle = player.transform.rotation.eulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;

        speed = player.normalSpeed;
        targetAngle = Mathf.Lerp(currentAngle, _direction.x * -player.maxInclineAngle, Mathf.Abs(_direction.x));

        Debug.Log(targetAngle);

        if (_direction.x > 0)
        {

            if (isPumping)
            {
                //Flow Animation
                //add Flow to the meter
                buttonHoldTime += Time.fixedDeltaTime;
                // Calculate carve force based on the button hold time
                speed = Mathf.Lerp(speed, player.pumpSpeed, buttonHoldTime / timeToMaxPump);
                targetAngle = Mathf.Lerp(currentAngle, _direction.x * -player.maxInclineBoostAngle * speed / player.pumpSpeed, buttonHoldTime / timeToMaxPump);
            }

        }
        else if (_direction.x < 0)
        {
            if (isPumping && speed > player.normalSpeed)
            {
                //Flow Animation
                //add Flow to the meter
                buttonHoldTime += Time.fixedDeltaTime;
                speed = Mathf.Lerp(speed, player.pumpSpeed, buttonHoldTime / timeToMaxPump);
                targetAngle = Mathf.Lerp(currentAngle, _direction.x * -player.maxInclineBoostAngle * speed / player.pumpSpeed, buttonHoldTime / timeToMaxPump);
            }

        }

        player.transform.position += new Vector3(0, -_direction.x, 0) * speed * Time.fixedDeltaTime;

        player.transform.rotation = Quaternion.Euler(0, 0, targetAngle);

    }

    public override void StateUpdate()
    {

    }

    public override void HandlePumping()
    {
        isPumping = true;
        //Play Pump animation in loop
    }

    public override void HandleStopPumping()
    {
        buttonHoldTime = 0f;
        isPumping = false;
        //Play Idle animation in loop
    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = dir;
    }


    public override void HandleTransition()
    {
        isPumping = false;
        base.HandleTransition();
    }
}