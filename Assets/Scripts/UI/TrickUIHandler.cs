using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickUIHandler : MonoBehaviour
{
    [SerializeField] private TrickUISetup trickUIPrefab;
    [SerializeField] private GameObject trickUI;
    [SerializeField] private GameObject trickUIParent;
    [SerializeField] private TMPro.TextMeshProUGUI trickTime;
    [SerializeField] private Slider trickTimeSlider;
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

    }

    private void Start()
    {

    }

    private void UpdateTimer(float timer)
    {
        float remainingTime = timer * maxTimer;
        trickTime.text = ((int)remainingTime).ToString();
        trickTimeSlider.value = remainingTime;
    }

    public void Initialize(TrickManager trickManager)
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

    public void ToggleOnUIPanel(TrickManager trickManager)
    {
        trickUI.SetActive(true);
    }

    public void ToggleUIOffPanel(TrickManager trickManager)
    {
        trickUI.SetActive(false);
    }

    public void SpawnTrickUIBox(TrickSO trickSO)
    {
        TrickUISetup trickUISetup = Instantiate(trickUIPrefab, trickUIParent.transform);
        trickUISetup.trickSO = trickSO;
        trickDictionary.Add(trickSO, trickUISetup);
        trickUISetup.SetupTrick(trickSO);
    }

    private void UpdateUI(TrickManager trickManager)
    {
        foreach (TrickSO trick in trickManager.tricks)
        {
            if (trickManager.possibleTrickCombos.Contains(trick))
                trickDictionary[trick].gameObject.SetActive(true);
            else
                trickDictionary[trick].gameObject.SetActive(false);
        }
    }

    public void ResetUI(TrickManager trickManager)
    {
        foreach (TrickSO trick in trickManager.tricks)
        {
            trickDictionary[trick].gameObject.SetActive(true);
        }
    }
}
