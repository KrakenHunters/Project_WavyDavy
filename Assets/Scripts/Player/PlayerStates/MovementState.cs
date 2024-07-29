using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MovementState : BaseState
{

    private bool isPumping = false;

    private float buttonHoldTime = 0f;
    private float buttonReleaseTime = 0f;

    private float pumpHoldTime;

    private float maxHeight;
    private float minHeight = -7.5f;

    private float timeToMaxPump;
    private float timeToMaxSpeed;
    private float timeToReturnSpeed;

    private float resetTimer;

    float targetAngle;
    float currentAngle;
    float speed;

    float holdFactor = 0f;


    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerMovement();

        switch (player.currentGamePhase)
        {
            case GamePhase.Phase2:
                maxHeight = player.phase2MaxHeight;
                timeToMaxPump = 0.6f;
                timeToMaxSpeed = 0.1f;
                timeToReturnSpeed = 10f;
                break;
            case GamePhase.Phase3:
                maxHeight = player.phase3MaxHeight;
                timeToMaxPump = 0.8f;
                timeToMaxSpeed = 0.1f;
                timeToReturnSpeed = 15f;
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

        // Check the player's height
        float playerHeight = player.transform.position.y;

        if (_direction.x > 0)
        {
            resetTimer = 0f;
            speed = Mathf.Lerp(speed, player.normalSpeed, _direction.x);
            targetAngle = _direction.x * -player.maxInclineAngle;


            if (isPumping)
            {
                pumpHoldTime += Time.fixedDeltaTime;
                // Flow Animation
                // Add Flow to the meter
                speed = Mathf.Lerp(speed, player.pumpSpeed, pumpHoldTime / timeToMaxPump);
                targetAngle = Mathf.Lerp(currentAngle, -player.maxInclineBoostAngle * (speed / player.pumpSpeed), pumpHoldTime / timeToMaxPump);
            }

            if (playerHeight <= minHeight)
            {
                speed = 0f;
                targetAngle = 0f;
                player.Event.OnIncreaseFlow.Invoke(-0.5f);

                //TransitionToCrashState and get back one phase
            }

        }
        else
        {
            resetTimer += Time.fixedDeltaTime;

            if (playerHeight >= maxHeight)
            {
                speed = 0f;
                targetAngle = 0f;
            }
            else
            {
                speed = Mathf.Lerp(speed, player.normalSpeed, resetTimer / timeToReturnSpeed);
                targetAngle = _direction.x * -player.maxInclineBoostAngle * speed / player.pumpSpeed;
            }


            targetAngle = _direction.x * -player.maxInclineAngle;
        }

        player.transform.position += new Vector3(0, -_direction.x, 0) * speed * Time.fixedDeltaTime;

        player.transform.rotation = Quaternion.Euler(0, 0, targetAngle);

    }
    public override void StateUpdate()
    {
        if (speed > player.normalSpeed && !isPumping)
        {
            player.Event.OnIncreaseFlow.Invoke((speed - player.normalSpeed) * 0.08f * Time.deltaTime);
        }


    }

    public override void HandlePumping()
    {
        isPumping = true;
        //Play Pump animation in loop
    }

    public override void HandleStopPumping()
    {
        pumpHoldTime = 0f;
        isPumping = false;
        //Play Idle animation in loop
    }

    public override void HitObject()
    {
        isPumping = false;
    }

    public override void HandleMovement(Vector2 dir)
    {

        if (dir.x > 0)
        {
            buttonReleaseTime = 0f;
            buttonHoldTime += Time.deltaTime;
            holdFactor = Mathf.Clamp(buttonHoldTime / timeToMaxSpeed, 0f, 1f);

            _direction = new Vector2(Mathf.Sign(dir.x) * holdFactor, 0f);
        }
        else if (dir.x < 0 && !isPumping)
        {
            buttonReleaseTime = 0f;
            buttonHoldTime += Time.deltaTime;
            holdFactor = Mathf.Clamp(buttonHoldTime / timeToMaxSpeed, 0f, 1f);

            _direction = new Vector2(Mathf.Sign(dir.x) * holdFactor, 0f);
        }
        else if (!isPumping)
        {
            buttonReleaseTime += Time.deltaTime;
            holdFactor = Mathf.Clamp(buttonReleaseTime / timeToMaxSpeed, 0f, 1f);

            float adjustedX = Mathf.Lerp(_direction.x, 0, holdFactor);
            _direction = new Vector2(adjustedX, 0f);
        }

    }


    public override void HandleTransition()
    {
        isPumping = false;
        base.HandleTransition();
    }
}