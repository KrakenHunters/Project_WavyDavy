using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public GameEvent Event;

    private float maxFlow;
    private float minFlow;
    private GamePhase nextPhase;
    private GamePhase? previousPhase;

    public float currentFlow { get; private set; }

    [SerializeField]
    private float maxFlowPhase1;
    [SerializeField]
    private float maxFlowPhase2;
    [SerializeField]
    private float maxFlowPhase3;

    [SerializeField]
    private float minFlowPhase1;
    [SerializeField]
    private float minFlowPhase2;

    [Range(0f, 1f)]
    [SerializeField]
    private float aboveThreshold;
    [Range(0f, 1f)]
    [SerializeField]
    private float belowThreshold;

    private float flowDecreaseSpeed; // Speed at which flow decreases

    [SerializeField]
    private float flowDecreaseSpeed1;
    [SerializeField]
    private float flowDecreaseSpeed2;
    [SerializeField]
    private float flowDecreaseSpeed3;

    [SerializeField]
    private float thresholdDuration; // Duration to check if flow stays within threshold

    [SerializeField]
    private FlowUIHandler flowUIHandler;

    private Coroutine thresholdCoroutine;
    private float previousFlow;
    private bool wasPhaseTrick;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ResetFlow);
        Event.OnHitObject.AddListener(AddFlow);
        Event.OnIncreaseFlow.AddListener(AddFlow);
        Event.OnGameEnd += DisableFlow;
        Event.OnFinishTransition.AddListener(RestartCoroutine);

        Event.OnTrickFinish += TrickCompleteCost;
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.AddListener(ResetFlow);
        Event.OnHitObject.RemoveListener(AddFlow);
        Event.OnIncreaseFlow.RemoveListener(AddFlow);
        Event.OnGameEnd -= DisableFlow;

        Event.OnFinishTransition.RemoveListener(RestartCoroutine);

        Event.OnTrickFinish -= TrickCompleteCost;

    }

    private void Start()
    {
        maxFlow = maxFlowPhase1;
        minFlow = minFlowPhase1;
        currentFlow = minFlowPhase1;
        previousPhase = null;
        nextPhase = GamePhase.Phase2;
        StartCoroutine(FlowDecrease());
    }


    private void Update()
    {
        flowUIHandler.UpdateFlowUI(currentFlow);
        if (previousPhase != GamePhase.Phase3)
            Event.OnFlowChange.Invoke(currentFlow);
    }


    private IEnumerator FlowDecrease()
    {

        while (true)
        {
            if (currentFlow >= maxFlow && nextPhase == GamePhase.Phase2)
            {
                Event.OnChangeGameState.Invoke(GamePhase.Phase2);
                StopAllCoroutines();
            }

            if (currentFlow > minFlow)
            {
                currentFlow -= flowDecreaseSpeed * (maxFlow - minFlow) * Time.deltaTime;
                currentFlow = Mathf.Max(currentFlow, minFlow); // Ensure currentFlow doesn't go below minFlow
            }

            bool isAboveThreshold = currentFlow > (maxFlow - (maxFlow - minFlow) * (1 - aboveThreshold));
            bool isBelowThreshold = currentFlow < ((maxFlow - minFlow) * belowThreshold + minFlow);

            Event.OnIsTrickPossible.Invoke(isAboveThreshold && GameManager.Instance.currentGamePhase == GamePhase.Phase3);



            if (isAboveThreshold && nextPhase != GamePhase.Trick)
            {
                if (thresholdCoroutine == null)
                {
                    thresholdCoroutine = StartCoroutine(CheckThresholdDuration(true));
                }
            }
            else if (isBelowThreshold && previousPhase.HasValue)
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
            if (isAboveThreshold && currentFlow <= (maxFlow - (maxFlow - minFlow) * (1 - aboveThreshold)))
            {
                yield break;
            }
            else if (!isAboveThreshold && currentFlow >= (maxFlow - minFlow) * belowThreshold + minFlow)
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
        if(previousPhase == GamePhase.Phase3)
        {
            StopAllCoroutines();
        }
    }

    private void ResetFlow(GamePhase newPhase)
    {
        previousFlow = currentFlow;
        StopAllCoroutines();
        switch (newPhase)
        {
            case GamePhase.Phase1:
                maxFlow = maxFlowPhase1;
                minFlow = minFlowPhase1;
                currentFlow = minFlowPhase1;
                previousPhase = null;
                nextPhase = GamePhase.Phase2;
                flowDecreaseSpeed = flowDecreaseSpeed1;
                flowUIHandler.CheckUIFow(false);

                break;
            case GamePhase.Phase2:
                maxFlow = maxFlowPhase2;
                minFlow = minFlowPhase2;
                currentFlow = (maxFlow - minFlow) / 2 + minFlow;
                previousPhase = GamePhase.Phase1;
                nextPhase = GamePhase.Phase3;
                flowDecreaseSpeed = flowDecreaseSpeed2;
                flowUIHandler.CheckUIFow(true);
                break;
            case GamePhase.Phase3:
                maxFlow = maxFlowPhase3;
                minFlow = maxFlowPhase2;
                currentFlow = wasPhaseTrick ? currentFlow : (maxFlow - minFlow) / 2 + minFlow;
                wasPhaseTrick = false;
                previousPhase = GamePhase.Phase2;
                nextPhase = GamePhase.Trick;
                flowDecreaseSpeed = flowDecreaseSpeed3;
                flowUIHandler.CheckUIFow(true);
                break;
            case GamePhase.Trick:
                //Disable UI
                flowUIHandler.CheckUIFow(false);
                wasPhaseTrick = true;
                previousPhase = GamePhase.Phase3;
                break;
        }
        flowUIHandler.SetMinMax(minFlowPhase2, maxFlowPhase3);

        //Update UI here

    }

    private void TrickCompleteCost(PlayerTrickHandler trickHandler)
    {
        if(trickHandler.CurrentResult == Tricks.TrickResult.Complete)
        {
            AddFlow(-(maxFlow-minFlow) * 0.7f);
        }
        else
        {
            AddFlow(-(maxFlow - minFlow) * 0.9f);
        }
    }

    private void DisableFlow()
    {
        StopAllCoroutines();
        currentFlow = 5f;

    }

    private void AddFlow(float flowAmount)
    {
        currentFlow += flowAmount;
        currentFlow = Mathf.Min(currentFlow, maxFlow); // Ensure currentFlow doesn't exceed maxFlow
        currentFlow = Mathf.Max(currentFlow, minFlow);
    }


}
