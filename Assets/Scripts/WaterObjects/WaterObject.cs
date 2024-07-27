using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterObject : MonoBehaviour , IHitable
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float flow;
    [SerializeField] private float deadZone = -25f;

    public GameEvent Event;

    public float Flow
    {
      get => flow;
      private set => flow = value;
    }

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
       // transform.position += Vector3.left * _speed * Time.deltaTime;
 
        transform.Translate(Vector3.left * _speed * Time.fixedDeltaTime);
        if (transform.position.x < deadZone)
        {
            Event.OnReachDeadZone.Invoke(this);
        }
    }

    public virtual void OnHit() 
    {
        Event.OnHitObject.Invoke(flow);
        Event.OnReachDeadZone.Invoke(this);
    }
}

public interface IHitable
{
    void OnHit();
}