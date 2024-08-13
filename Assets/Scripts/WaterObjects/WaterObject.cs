using UnityEngine;

public class WaterObject : MonoBehaviour, IHitable
{
    private float objSpeed;

    [SerializeField] private float _speedMod = 1f;
    [SerializeField] private float flow;
    [SerializeField] private float deadZone = -25f;


    [SerializeField] private AudioClip hitClip;

    public GameEvent Event;

    private Collider2D Collider;

    public float Flow
    {
        get => flow;
        private set => flow = value;
    }

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        Event.OnFlowChange.AddListener(FlowChange);
        Event.OnChangeGameState.AddListener(SinkObject);
        Collider.enabled = true;
    }

    private void OnDisable()
    {
        Event.OnFlowChange.RemoveListener(FlowChange);
        Event.OnChangeGameState.RemoveListener(SinkObject);

    }

    private void FlowChange(float currentFlow)
    {
        objSpeed = currentFlow * _speedMod;
    }

    private void SinkObject(GamePhase phase)
    {
        Collider.enabled = false;
        //Have an animation on the object that gets "submersed" in the water, sinking in the water
        //After animation is done:
        Event.OnReachDeadZone.Invoke(this);
    }

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
        // transform.position += Vector3.left * _speed * Time.deltaTime;

    }

    private void Update()
    {
        transform.Translate(Vector3.left * objSpeed * Time.deltaTime);
        if (transform.position.x < deadZone)
        {
            Event.OnReachDeadZone.Invoke(this);
        }

    }

    public virtual void OnHit()
    {
        AudioManager.Instance.PlayAudio(hitClip);
        Event.OnHitObject.Invoke(flow);
        Event.OnReachDeadZone.Invoke(this);
    }
}

public interface IHitable
{
    void OnHit();
}