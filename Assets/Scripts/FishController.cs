using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private float timerMax;
    [SerializeField] private float timerMin;

    private float timer = 0f;
    private float timeLimit;

    [SerializeField]
    private Animator fishAnimator;
    protected static readonly int SharkJump = Animator.StringToHash("shark");


    private void Start()
    {
        SetTimer();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if ( timer > timeLimit )
        {
            SetTimer(); 
            timer = 0f;
            fishAnimator.Play(SharkJump);
        }

    }

    private void SetTimer()
    {
        timeLimit = Random.Range(timerMin, timerMax );
    }
}
