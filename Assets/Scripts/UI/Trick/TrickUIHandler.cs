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
    [SerializeField, Range(0, 1)] private float finalFadeAmount;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private CanvasGroup trickUICanvasGroup;

    private Tween tween;
    private Dictionary<TrickSO, TrickUISetup> trickDictionary = new();

    private float maxTimer;

    public GameEvent Event;

    private void OnEnable()
    {
        Event.OnTrickInput += UpdateUI;

        Event.OnTrickStart += ToggleOnUIPanel;
        Event.OnTrickStart += ResetUI;

        Event.OnTrickRunning += UpdateTimer;

        Event.OnTrickFinish += ToggleUIOffPanel;

        Event.OnTrickHalfTime += StartFadingOG;

    }

    private void OnDisable()
    {
        Event.OnTrickInput -= UpdateUI;
        Event.OnTrickStart -= ToggleOnUIPanel;
        Event.OnTrickStart -= ResetUI;
        Event.OnTrickRunning -= UpdateTimer;
        Event.OnTrickFinish -= ToggleUIOffPanel;
        Event.OnTrickHalfTime -= StartFadingOG;
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

    public void StartFadingOG()
    {
        if (tween.IsActive()) return;
        FadeOut();
    }

    private void FadeOut()
    {
        tween = trickUICanvasGroup.DOFade(finalFadeAmount, fadeDuration).SetEase(ease);
    }

    public void StartBlinking(List<TrickSO> so) => StartFadingOG();

    public void StopBlinking()
    {
        tween.Kill();
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
