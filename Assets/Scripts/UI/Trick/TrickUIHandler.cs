using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TrickUIHandler : MonoBehaviour
{
    [SerializeField] private TrickUISetup trickUIPrefab;
    [SerializeField] private GameObject trickUI;
    [SerializeField] private GameObject trickUIParent;
    [SerializeField] private TMPro.TextMeshProUGUI trickTime;
    [SerializeField] private Slider trickTimeSlider;

    [Header("Trick Fade Settings")]
    [SerializeField] private float fadeDuration;
    [SerializeField] private float fadeAmount;
    [SerializeField] private CanvasGroup trickUICanvasGroup;
    private float currentDuration;
    private float currentFadeAmount;

    private Dictionary<TrickSO, TrickUISetup> trickDictionary = new();

    private float maxTimer;
    private bool isBlinking;

    public GameEvent Event;

    private void OnEnable()
    {
        Event.OnTrickInput += UpdateUI;

        Event.OnTrickStart += ToggleOnUIPanel;
        Event.OnTrickStart += ResetUI;

        Event.OnTrickRunning += UpdateTimer;

        Event.OnTrickFinish += ToggleUIOffPanel;

        Event.OnTrickHalfTime += StartBlinkingOG;
     
    }

    private void OnDisable()
    {
        Event.OnTrickInput -= UpdateUI;
        Event.OnTrickStart -= ToggleOnUIPanel;
        Event.OnTrickStart -= ResetUI;
        Event.OnTrickRunning -= UpdateTimer;
        Event.OnTrickFinish -= ToggleUIOffPanel;
    }

    private void UpdateTimer(float timer)
    {
        float remainingTime = timer * maxTimer;
        trickTime.text = ((int)remainingTime).ToString();
        trickTimeSlider.value = remainingTime;
    }

    public void Initialize(PlayerTrickHandler trickManager)
    {
        foreach (TrickSO trick in trickManager.tricks)
        {
            SpawnTrickUIBox(trick);
        }
        currentDuration = fadeDuration;
        currentFadeAmount = 0.5f;
        maxTimer = trickManager.MaxTrickTime;
        trickTimeSlider.maxValue = maxTimer;
        trickTimeSlider.value = trickManager.MaxTrickTime; // Initialize slider to max value
        trickTime.text = ((int)trickManager.MaxTrickTime).ToString();
    }

    public void ToggleOnUIPanel(PlayerTrickHandler x)
    {
        trickUI.SetActive(true);
        Event.OnTrickInput += StartBlinking;

    }

    public void ToggleUIOffPanel(PlayerTrickHandler x)
    {
        StopBlinking();
        trickUI.SetActive(false);
        Event.OnTrickInput -= StartBlinking;
    }

    public void SpawnTrickUIBox(TrickSO trickSO)
    {
        if (trickSO == null) return;
        if (!trickDictionary.ContainsKey(trickSO))
        {
            TrickUISetup trickUISetup = Instantiate(trickUIPrefab, trickUIParent.transform);
            if (trickUISetup != null)
            {
                trickDictionary.Add(trickSO, trickUISetup);
                trickUISetup.SetupTrick(trickSO);
            }
        }
    }


    private IEnumerator FadeInOut()
    {
        isBlinking = true;
        while (isBlinking)
        {
            yield return StartCoroutine(FadeCanvasGroup(currentFadeAmount, 0f));
            if (currentFadeAmount < fadeAmount)
                currentFadeAmount += 0.1f;
            yield return StartCoroutine(FadeCanvasGroup(0f, currentFadeAmount));
        }
    }

    private IEnumerator FadeCanvasGroup(float start, float end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < currentDuration)
        {
            trickUICanvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / currentDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (currentDuration > 0.5f)
            currentDuration -= 0.1f;

        trickUICanvasGroup.alpha = end;
    }

    public void StartBlinkingOG()
    {
        if (isBlinking) return;
        StartCoroutine(FadeInOut());
    }

    public void StartBlinking(List<TrickSO> so) => StartBlinkingOG();

    public void StopBlinking()
    {
        StopAllCoroutines();
        isBlinking = false;
        currentDuration = fadeDuration;
        trickUICanvasGroup.alpha = 1;
    }

    private void UpdateUI(List<TrickSO> possibleTricks)
    {
        foreach (KeyValuePair<TrickSO, TrickUISetup> trickValues in trickDictionary)
        {
            if (possibleTricks.Contains(trickValues.Key))
                trickValues.Value.gameObject.SetActive(true);
            else
                trickValues.Value.gameObject.SetActive(false);
        }
    }

    public void ResetUI(PlayerTrickHandler trickManager)
    {
        foreach (TrickUISetup trickUISetup in trickDictionary.Values)
        {
            trickUISetup.gameObject.SetActive(true);
        }
    }
}
