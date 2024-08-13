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

    [Header("Trick Can Do")]
    [SerializeField] private UIAnimator canDoTrick;
    [SerializeField] private ParticleSystem arrowOnPlayer;

    [Header("Trick Arrows color")]
    [SerializeField] private Color LitColor;
    [SerializeField] private Color unlitColor;
    [SerializeField] private Color fadedColor;


    public int PlayerInputCount { get; set; }

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

        Event.OnIsTrickPossible += UpdateCanDoTrickUI;

    }



    private void OnDisable()
    {
        Event.OnTrickInput -= UpdateUI;
        Event.OnTrickStart -= ToggleOnUIPanel;
        Event.OnTrickStart -= ResetUI;
        Event.OnTrickRunning -= UpdateTimer;
        Event.OnTrickFinish -= ToggleUIOffPanel;
        Event.OnTrickHalfTime -= StartFadingOG;
        Event.OnIsTrickPossible -= UpdateCanDoTrickUI;
    }

    private void UpdateCanDoTrickUI(bool show)
    {
        canDoTrick.gameObject.SetActive(show);
        arrowOnPlayer.gameObject.SetActive(show);
        if (show)
        {
            canDoTrick.FadeAnimate();
        }

    }

    private void UpdateTimer(float timer)
    {
        float remainingTime = timer * maxTimer;
        trickTime.text = remainingTime <= 0.1f ? 0f.ToString() : Mathf.Ceil(remainingTime).ToString();
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
        trickTime.text = Mathf.Ceil(trickManager.MaxTrickTime).ToString();
    }

    public void ToggleOnUIPanel(PlayerTrickHandler x)
    {
        trickUI.SetActive(true);

    }

    public void ToggleUIOffPanel(PlayerTrickHandler x)
    {
        StopBlinking();
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
                trickUISetup.SetupTrick(trickSO, unlitColor);
                trickUISetup.gameObject.SetActive(true);
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
                trickValues.Value.HighLightArrows(PlayerInputCount, LitColor);
            else
                trickValues.Value.UnHighLightAllArrows(fadedColor);
        }
    }

    public void ResetUI(PlayerTrickHandler trickManager)
    {
        foreach (TrickUISetup trickUISetup in trickDictionary.Values)
        {
            trickUISetup.UnHighLightAllArrows(unlitColor);
        }
    }
}
