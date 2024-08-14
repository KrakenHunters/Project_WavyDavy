using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsBubblePop : MonoBehaviour
{
    private static readonly int Pop = Animator.StringToHash("Pop");
    public GameEvent Event;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(Pop);
        Destroy(this.gameObject, 2f);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Pop)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                Event.OnReachDeadZone.Invoke(this.GetComponentInParent<WaterObject>());
        }

    }
}
