using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MovementState : BaseState
{
    bool isPumping = false;
    float buttonHoldTime = 0f;
    float buttonReleaseTime = 0f;
    float maxHeight;
    float minHeight = -7.5f;
    float timeToMaxPump;
    float timeToMaxSpeed;
    float timeToReturnSpeed;
    float timeToReturnAngle;
    float timeToNoSpeed = 0.2f;
    float resetTimer;
    float targetAngle;
    float currentAngle;
    float speed;
    float holdFactor = 0f;
    float possibleXMovement;
    float startXPos;
    Vector3 idleMovement;
    Vector3 originalPosition;
    float idleTimer = 0f;
    float direction = 0f;
    bool speeding;
    public override void EnterState()
    {
        base.EnterState();
        inputManager.EnablePlayerMovement();
        originalPosition = player.transform.position;
        InitializeGamePhaseSettings();
    }
    private void InitializeGamePhaseSettings()
    {
        switch (player.currentGamePhase)
        {
            case GamePhase.Phase2:
                maxHeight = player.phase2MaxHeight;
                timeToMaxPump = 2f;
                possibleXMovement = 8f;
                timeToMaxSpeed = 0.1f;
                timeToReturnSpeed = 20f;
                timeToReturnAngle = 1f;
                break;
            case GamePhase.Phase3:
                maxHeight = player.phase3MaxHeight;
                timeToMaxPump = 3f;
                possibleXMovement = 15f;
                timeToMaxSpeed = 0.2f;
                timeToReturnSpeed = 60f;
                timeToReturnAngle = 1f;
                break;
        }
    }

    public override void ExitState() { }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        currentAngle = GetCurrentAngle();
        HandleMovementLogic();
        ApplyRandomIdleMovement();
        ApplyForces();
        if (speed <= player.normalSpeed || _direction.x >= 0f)
        {
            speeding = false;
            player.windAnimator.SetActive(false);

        }
    }

    public override void StateUpdate() { }

    public override void HitObject()
    {
        isPumping = false;
        speed = 0f;
        player.animator.CrossFade(GetHit, 0.2f);
    }

    public override void HandleMovement(Vector2 dir)
    {
        UpdateDirection(dir);
        UpdateHoldFactor(dir);
    }

    public override void HandleTransition()
    {
        isPumping = false;
        speed = player.normalSpeed;
        base.HandleTransition();
    }

    private float GetCurrentAngle()
    {
        float angle = player.transform.rotation.eulerAngles.z;
        return angle > 180 ? angle - 360 : angle;
    }

    private void HandleMovementLogic()
    {
        float playerHeight = player.transform.position.y;
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, originalPosition.z);

        if (_direction.x > 0)
        {
            speeding = false;
            if (startXPos > player.transform.position.x)
            {
                startXPos = player.transform.position.x;
            }
            resetTimer = 0f;
            speed = Mathf.Lerp(speed, player.normalSpeed, (buttonHoldTime) / timeToMaxSpeed);
            targetAngle = _direction.x * -player.maxInclineAngle;

            if (isPumping)
            {
                if (player.animator.GetCurrentAnimatorStateInfo(0).shortNameHash != Pump)
                    player.animator.Play(Pump);
                speed = Mathf.Lerp(speed, player.pumpSpeed, (buttonHoldTime - timeToMaxSpeed) / timeToMaxPump);
                targetAngle = Mathf.Lerp(currentAngle, -player.maxInclineAngle * (speed / player.pumpSpeed), (buttonHoldTime - timeToMaxSpeed) / timeToMaxPump);
                targetPosition.x = Mathf.Lerp(startXPos, originalPosition.x + possibleXMovement, (buttonHoldTime - timeToMaxSpeed) / timeToMaxPump);
                player.transform.position = targetPosition;
            }

            if (playerHeight <= minHeight)
            {
                HandleMinHeightCollision();
            }
        }
        else
        {
            player.animator.CrossFade(Idle, 0.2f);

            if (startXPos < player.transform.position.x)
            {
                startXPos = player.transform.position.x;
            }

            HandleResetLogic(playerHeight);

        }

    }

    private void HandleMinHeightCollision()
    {
        speed = 0f;
        targetAngle = 0f;
        player.Event.OnIncreaseFlow.Invoke(-0.5f);
        AudioManager.Instance.PlayAudio(player.speedPlayer);

    }

    private void HandleResetLogic(float playerHeight)
    {
        resetTimer += Time.fixedDeltaTime;

        float resetDelay = 0.2f;
        if (playerHeight >= maxHeight)
        {
            HandleMaxHeight(playerHeight);
        }
        else
        {
            if (speed > player.normalSpeed && _direction.x < 0f && !speeding)
            {
                AudioManager.Instance.PlayAudio(player.speedPlayer);
                player.windAnimator.SetActive(true);
                speeding = true;
            }
            if (resetTimer > resetDelay)
            {
                speed = Mathf.Lerp(speed, player.normalSpeed, (resetTimer - resetDelay) / timeToReturnSpeed);
            }
            targetAngle = Mathf.Lerp(currentAngle, _direction.x * -player.maxInclineBoostAngle * speed / player.pumpSpeed, (resetTimer) / timeToReturnAngle);
            HandleResetXPosition(resetTimer);
        }
    }

    private void HandleResetXPosition(float timer)
    {
        float XPos = Mathf.Lerp(player.transform.position.x, originalPosition.x, timer / (timeToReturnSpeed + 100f));
        player.transform.position = new Vector3(XPos, player.transform.position.y, originalPosition.z);  // Move back to original x position
    }

    private void HandleMaxHeight(float playerHeight)
    {
        if (speed > player.normalSpeed && player.canTrick)
        {
            player.Event.OnChangeGameState.Invoke(GamePhase.Trick);
        }
        else
        {
            speed = 0f;
            targetAngle = 0f;
        }
    }

    private void ApplyRandomIdleMovement()
    {
        if (_direction == Vector3.zero)
        {
            idleTimer += Time.fixedDeltaTime;
            float idleX = Mathf.PerlinNoise(idleTimer, 0) - 0.1f;
            float idleY = Mathf.PerlinNoise(0, idleTimer) - 0.5f;
            idleMovement = new Vector3(idleX * 0.08f, idleY * 0.05f, 0) * 0.5f;
            player.transform.position += idleMovement + Vector3.down * 0.005f;
        }
    }

    private void ApplyForces()
    {
        if (speed > player.normalSpeed && !isPumping && _direction.x < 0)
        {
            player.Event.OnIncreaseFlow.Invoke((speed - player.normalSpeed) * 2f * Time.fixedDeltaTime);
        }

        player.transform.position += new Vector3(0, -_direction.x, 0) * speed * Time.fixedDeltaTime;
        player.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    private void UpdateDirection(Vector2 dir)
    {
        if (dir.x != 0)
        {
            if (Mathf.Sign(dir.x) != direction)
            {
                buttonHoldTime = 0f;
                direction = Mathf.Sign(dir.x);
            }
            else
            {
                buttonHoldTime += Time.deltaTime;
            }

            _direction = new Vector2(Mathf.Sign(dir.x) * holdFactor, 0f);
        }
        else
        {
            buttonHoldTime = 0f;
            buttonReleaseTime += Time.deltaTime;
            holdFactor = Mathf.Clamp(buttonReleaseTime / timeToNoSpeed, 0f, 1f);
            _direction = new Vector2(Mathf.Lerp(_direction.x, 0, holdFactor), 0f);
        }
        UpdatePumpingStatus(_direction);
    }

    private void UpdateHoldFactor(Vector2 dir)
    {
        holdFactor = Mathf.Clamp(buttonHoldTime / timeToMaxSpeed, 0f, 1f);
    }

    private void UpdatePumpingStatus(Vector2 dir)
    {
        isPumping = buttonHoldTime >= timeToMaxSpeed && dir.x >= 1;
    }
}
