using System.Collections;
using System.Collections.Generic;
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
    }

    private void OnDisable()
    {
        Event.OnFlowChange.RemoveListener(FlowChange);
    }

    private void Awake()
    {
        background = GetComponent<MeshRenderer>();
    }

    private void FlowChange(float currentFlow)
    {
        animationSpeed = currentFlow * animationConvert;
    }

    // Update is called once per frame
    void Update()
    {
        background.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }

}