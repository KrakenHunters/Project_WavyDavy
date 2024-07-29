using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MovementState : BaseState
{

    private bool isPumping = false;

    private float buttonHoldTime = 0f;
    private float buttonReleaseTime = 0f;

    private float maxHeight;
    private float minHeight = -7.5f;

    private float timeToMaxPump;
    private float timeToMaxSpeed;
    private float timeToReturnSpeed;
    private float timeToReturnAngle;
    private float timeToNoSpeed = 0.2f;

    private float resetTimer;

    float targetAngle;
    float currentAngle;
    float speed;

    float holdFactor = 0f;

    private Vector3 idleMovement;
    private Vector3 originalPosition;
    private float idleTimer = 0f;

    private float direction = 0f;


    public override void EnterState()
    {
        base.EnterState();
        
        inputManager.EnablePlayerMovement();

        originalPosition = player.transform.position;

        switch (player.currentGamePhase)
        {
            case GamePhase.Phase2:
                maxHeight = player.phase2MaxHeight;
                timeToMaxPump = 0.4f;
                timeToMaxSpeed = 0.1f;
                timeToReturnSpeed = 15f;
                timeToReturnAngle = 3f;
                break;
            case GamePhase.Phase3:
                maxHeight = player.phase3MaxHeight;
                timeToMaxPump = 0.5f;
                timeToMaxSpeed = 0.1f;
                timeToReturnSpeed = 20f;
                timeToReturnAngle = 3f;

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

        Vector3 targetPosition = new Vector3(originalPosition.x, player.transform.position.y, originalPosition.z);

        if (_direction.x > 0)
        {
            resetTimer = 0f;
            speed = Mathf.Lerp(speed, player.normalSpeed, _direction.x);
            targetAngle = _direction.x * -player.maxInclineAngle;

            if (isPumping)
            {
                speed = Mathf.Lerp(speed, player.pumpSpeed, (buttonHoldTime - timeToMaxPump) / timeToMaxPump);
                targetAngle = Mathf.Lerp(currentAngle, -player.maxInclineBoostAngle * (speed / player.pumpSpeed), (buttonHoldTime - timeToMaxPump) / timeToMaxPump);

/*                float pumpFactor = Mathf.Clamp01((buttonHoldTime - timeToMaxPump) / timeToMaxPump);
                targetPosition.x = Mathf.Lerp(originalPosition.x, originalPosition.x + 0.3f, pumpFactor); // Smooth right movement
*/          }

            if (playerHeight <= minHeight)
            {
                speed = 0f;
                targetAngle = 0f;
                player.Event.OnIncreaseFlow.Invoke(-0.5f);

                // Get Hit animation
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
                targetAngle = Mathf.Lerp(currentAngle, _direction.x * -player.maxInclineBoostAngle * speed / player.pumpSpeed, resetTimer / timeToReturnAngle);
            }

            // Apply idle random movement and downward force
            if (_direction == Vector3.zero)
            {
                idleTimer += Time.fixedDeltaTime;

                float idleX = Mathf.PerlinNoise(idleTimer, 0) - 0.5f; // Smooth random x movement
                float idleY = Mathf.PerlinNoise(0, idleTimer) - 0.5f; // Smooth random y movement

                idleMovement = new Vector3(idleX, idleY * 0.05f, 0) * 0.5f;

                targetPosition += idleMovement;

                // Apply small downward force
                targetPosition += Vector3.down * 0.005f;
            }
        }

        if (speed > player.normalSpeed && !isPumping)
        {
            player.Event.OnIncreaseFlow.Invoke((speed - player.normalSpeed) * 0.1f * Time.fixedDeltaTime);
        }


        player.transform.position = targetPosition;
        player.transform.position += new Vector3(0, -_direction.x, 0) * speed * Time.fixedDeltaTime;
        player.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }
    public override void StateUpdate()
    {
        Debug.Log(speed);

    }

    public override void HitObject()
    {
        isPumping = false;
        speed = player.normalSpeed;
    }

    public override void HandleMovement(Vector2 dir)
    {

        if (dir.x != 0)
        {
            // If direction has changed (compared to the previous direction)
            if (Mathf.Sign(dir.x) != direction)
            {
                // Reset buttonHoldTime since the direction has changed
                buttonHoldTime = 0f;

                // Update the direction to the new direction
                direction = Mathf.Sign(dir.x);
            }
            else
            {
                // If direction hasn't changed, increment buttonHoldTime
                buttonHoldTime += Time.deltaTime;
            }

            // Calculate the holdFactor based on the time held
            holdFactor = Mathf.Clamp(buttonHoldTime / timeToMaxSpeed, 0f, 1f);

            // Determine if the button is being pumped based on the hold time
            if (buttonHoldTime > timeToMaxPump && dir.x >= 1)
            {
                isPumping = true;
            }
            else
            {
                isPumping = false;
            }

            // Update _direction based on the new direction and holdFactor
            _direction = new Vector2(Mathf.Sign(dir.x) * holdFactor, 0f);
        }
        else
        {
            buttonHoldTime = 0f;
            buttonReleaseTime += Time.deltaTime;
            holdFactor = Mathf.Clamp(buttonReleaseTime / timeToNoSpeed, 0f, 1f);

            float adjustedX = Mathf.Lerp(_direction.x, 0, holdFactor);
            _direction = new Vector2(adjustedX, 0f);
        }

    }


    public override void HandleTransition()
    {
        isPumping = false;
        speed = player.normalSpeed;
        base.HandleTransition();
    }
}