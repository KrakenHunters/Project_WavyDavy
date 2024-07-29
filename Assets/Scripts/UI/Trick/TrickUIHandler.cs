using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private Ease fadeEase;
    [SerializeField] private CanvasGroup trickUICanvasGroup;
        
    private Tween fadetween;

    private Dictionary<TrickSO, TrickUISetup> trickDictionary = new();

    private float maxTimer;
    [SerializeField] private GameEvent Event;

    private void OnEnable()
    {
        Event.OnTrickInput += UpdateUI;

        Event.OnTrickStart += ToggleOnUIPanel;
        Event.OnTrickStart += ResetUI;

        Event.OnTrickRunning += UpdateTimer;

        Event.OnTrickComplete += ToggleUIOffPanel;
        Event.OnTrickFail += ToggleUIOffPanel;
    }

    private void OnDisable()
    {
        Event.OnTrickFail -= ToggleUIOffPanel;
        Event.OnTrickInput -= UpdateUI;
        Event.OnTrickStart -= ToggleOnUIPanel;
        Event.OnTrickStart -= ResetUI;
        Event.OnTrickRunning -= UpdateTimer;
        Event.OnTrickComplete -= ToggleUIOffPanel;
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
    }

    public void ToggleUIOffPanel(PlayerTrickHandler x)
    {
        trickUI.SetActive(false);
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


    private void Fade(float newAlpha)
    {
        fadetween = trickUICanvasGroup.DOFade(newAlpha, fadeDuration).SetEase(fadeEase);
    }

    public void StartBlinking()
    {
        fadetween.onComplete += () =>
        {
            Fade(trickUICanvasGroup.alpha == fadeAmount ? fadeAmount : 1);
        };

    }
    public void StopBlinking()
    {
        fadetween.Kill();
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
