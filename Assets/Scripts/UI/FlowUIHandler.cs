using UnityEngine;
using UnityEngine.UI;

public class FlowUIHandler : MonoBehaviour
{
    [SerializeField] private Slider flowSlider;
    [SerializeField] private float speed = 5f;

    [SerializeField] private UIAnimator flowCanvas;
    [SerializeField] private UIAnimator trickUIflowImage;

    private float target = 0f;

    public void UpdateFlowUI(float flow) => target = flow;

    public void SetMinMax(float minFlow, float maxFlow)
    {
        flowSlider.minValue = minFlow;
        flowSlider.maxValue = maxFlow;
    }

    private void Update() => flowSlider.value = Mathf.Lerp(flowSlider.value, target, speed * Time.deltaTime);
    
    public void CheckUIFow(bool check)
    {
        flowCanvas.FadeInAnimate(check);
        trickUIflowImage.FadeInAnimate(check);
    }    
}