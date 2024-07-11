using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(BoxCollider2D))]
public class WaveScript : MonoBehaviour
{
    private PlayerController _character;
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private float backwardForce;

    [SerializeField]
    private float reducedBackwardForce;

    public GameEvent Event;

    private Animator _animator;
    private GamePhase currentPhase;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
    }

    private void SetPhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                if (currentPhase == GamePhase.Phase2)
                {
                    _animator.SetTrigger("Phase2To1");
                }
                break;
            case GamePhase.Phase2:
                if (currentPhase == GamePhase.Phase1)
                {
                    _animator.SetTrigger("Phase1To2");
                }
                else if (currentPhase == GamePhase.Phase3)
                {
                    _animator.SetTrigger("Phase3To2");
                }
                break;
            case GamePhase.Phase3:
                _animator.SetTrigger("Phase2To3");
                break;
        }
        currentPhase = newPhase;
    }
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
    void ApplyBackwardForce()
    {
        float currentBackwardForce = backwardForce;
        if (_character.MovementDirection.x > 0)
        {
            currentBackwardForce = reducedBackwardForce;
        }
        Vector3 newPosition = _character.transform.position - new Vector3(currentBackwardForce * Time.deltaTime, 0, 0);

        // Ensure the character stays within the wave collider bounds
        if (_boxCollider.bounds.Contains(newPosition))
        {
            _character.transform.position = newPosition;
        }
    }

    // Call this method whenever the wave size changes
    public void UpdateWaveCollider(Vector2 newSize, Vector2 newCenter)
    {
        _boxCollider.size = newSize;
        _boxCollider.offset = newCenter;
    }

    // Call this method whenever we need to disable or enable the collider on the wave
    public void EnableBoxCollider(bool enable)
    {
        _boxCollider.enabled = enable;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            _character = collision.GetComponent<PlayerController>();
            ApplyBackwardForce();
            // Constrain the character within the wave collider
            // Constrain the character within the wave collider
            Vector3 newPosition = _character.transform.position;
            if (newPosition.x < _boxCollider.bounds.min.x)
                newPosition.x = _boxCollider.bounds.min.x;
            if (newPosition.x > _boxCollider.bounds.max.x)
                newPosition.x = _boxCollider.bounds.max.x;
            if (newPosition.y < _boxCollider.bounds.min.y)
                newPosition.y = _boxCollider.bounds.min.y;
            if (newPosition.y > _boxCollider.bounds.max.y)
                newPosition.y = _boxCollider.bounds.max.y;
            _character.transform.position = newPosition;
        }
    }
}
