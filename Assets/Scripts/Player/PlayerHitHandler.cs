using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
/*    private Collider _hitCollider; // do we need this Renee?
    private void Awake() => _hitCollider = GetComponent<Collider>();
*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IHitable>(out IHitable obstacle))
        {
            obstacle.OnHit();
        }
    }
}
