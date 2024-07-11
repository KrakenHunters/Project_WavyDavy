using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour , IHitable
{
    [SerializeField] protected float _speed = 1f;

    [SerializeField] protected float flow;

    public GameEvent Event;

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public virtual void OnHit() 
    {
        Event.OnHit.Invoke(gameObject);
    }
}

public interface IHitable
{
    void OnHit();
}