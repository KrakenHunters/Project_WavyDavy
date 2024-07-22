using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public GameEvent Event;

    private float maxFlow;
    private float currentFlow;
    private GamePhase nextPhase;
    private GamePhase? previousPhase;


    [SerializeField]
    private float maxFlowPhase1;
    [SerializeField]
    private float maxFlowPhase2;
    [SerializeField]
    private float maxFlowPhase3;

    [Range(0f, 1f)]
    [SerializeField]
    private float aboveThreshold;
    [Range(0f, 1f)]
    [SerializeField]
    private float belowThreshold;

    [SerializeField]
    private float flowDecreaseSpeed; // Speed at which flow decreases
    [SerializeField]
    private float thresholdDuration; // Duration to check if flow stays within threshold

    [SerializeField]
    private FlowUIHandler flowUIHandler;

    private Coroutine thresholdCoroutine;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ResetFlow);
        Event.OnHitObject.AddListener(AddFlow);
        Event.OnFinishTransition.AddListener(RestartCoroutine);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ResetFlow);
        Event.OnHitObject.RemoveListener(AddFlow);
        Event.OnFinishTransition.RemoveListener(RestartCoroutine);
    }

    private void Start()
    {
        maxFlow = maxFlowPhase1;
        currentFlow = 0f;
        previousPhase = null;
        nextPhase = GamePhase.Phase2;
        StartCoroutine(FlowDecrease());
    }


    private void Update() => flowUIHandler.UpdateFlowUI(currentFlow);

    private IEnumerator FlowDecrease()
    {
        while (true)
        {
            if (currentFlow > 0)
            {
                currentFlow -= flowDecreaseSpeed * Time.deltaTime;
                currentFlow = Mathf.Max(currentFlow, 0); // Ensure currentFlow doesn't go below 0

                // Update UI here
            }

            if (currentFlow > maxFlow * aboveThreshold && nextPhase != GamePhase.Trick)
            {
                if (thresholdCoroutine == null)
                {
                    thresholdCoroutine = StartCoroutine(CheckThresholdDuration(true));
                }
            }
            else if (currentFlow < maxFlow * belowThreshold && previousPhase.HasValue)
            {
                if (thresholdCoroutine == null)
                {
                    thresholdCoroutine = StartCoroutine(CheckThresholdDuration(false));
                }
            }
            else
            {
                if (thresholdCoroutine != null)
                {
                    StopCoroutine(thresholdCoroutine);
                    thresholdCoroutine = null;
                }
            }
            yield return null;
        }
    }

    private IEnumerator CheckThresholdDuration(bool isAboveThreshold)
    {
        float timer = 0f;
        while (timer < thresholdDuration)
        {
            if (isAboveThreshold && currentFlow <= maxFlow * aboveThreshold)
            {
                yield break;
            }
            else if (!isAboveThreshold && currentFlow >= maxFlow * belowThreshold)
            {
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (isAboveThreshold && nextPhase != GamePhase.Trick) //Remove the trick condition later
        {
            Event.OnChangeGameState.Invoke(nextPhase);
        }
        else if (!isAboveThreshold && previousPhase.HasValue)
        {
            Event.OnChangeGameState.Invoke(previousPhase.Value);
        }

        thresholdCoroutine = null;
    }

    private void RestartCoroutine()
    {
        StartCoroutine(FlowDecrease());
    }

    private void ResetFlow(GamePhase newPhase)
    {
        StopAllCoroutines();
        switch (newPhase)
        {
            case GamePhase.Phase1:
                maxFlow = maxFlowPhase1;
                currentFlow = 0f;
                previousPhase = null;
                nextPhase = GamePhase.Phase2;
                break;
            case GamePhase.Phase2:
                maxFlow = maxFlowPhase2;
                currentFlow = maxFlow / 2;
                previousPhase = GamePhase.Phase1;
                nextPhase = GamePhase.Phase3;
                break;
            case GamePhase.Phase3:
                maxFlow = maxFlowPhase3;
                currentFlow = maxFlow / 2;
                previousPhase = GamePhase.Phase2;
                nextPhase = GamePhase.Trick;
                break;
            case GamePhase.Trick:
                //Disable UI
                previousPhase = GamePhase.Phase3;
                break;
        }
        flowUIHandler.SetMinMax(0f, maxFlow);

        //Update UI here

    }

    private void AddFlow(float flowAmount)
    {
        currentFlow += flowAmount;
        currentFlow = Mathf.Min(currentFlow, maxFlow); // Ensure currentFlow doesn't exceed maxFlow
        currentFlow = Mathf.Max(currentFlow, 0f);
    }


}
