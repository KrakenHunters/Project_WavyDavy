using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    private MeshRenderer background;
    // Start is called before the first frame update
    [SerializeField]
    private float animationConvert = 1f;

    private float animationSpeed;

    [SerializeField]
    private GameEvent Event;

    private void OnEnable()
    {
        Event.OnFlowChange.AddListener(FlowChange);
        Event.OnTrickStart += ApplySlowMo;

    }

    private void OnDisable()
    {
        Event.OnFlowChange.RemoveListener(FlowChange);
        Event.OnTrickStart -= ApplySlowMo;

    }

    private void Awake()
    {
        background = GetComponent<MeshRenderer>();
    }

    private void FlowChange(float currentFlow)
    {
        animationSpeed = currentFlow * animationConvert;
    }

    private void ApplySlowMo(PlayerTrickHandler trickHandler)
    {
        animationSpeed = 0.1f * animationConvert;
    }

    // Update is called once per frame
    void Update()
    {
        background.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }

}