using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
   Collider hitCollider;

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHitable>(out IHitable obstacle))
        {
            obstacle.OnHit();
        }
    }
}
