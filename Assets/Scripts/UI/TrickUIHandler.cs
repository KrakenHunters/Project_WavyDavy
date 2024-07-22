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

    private TrickManager trickManager;
    private Dictionary<TrickSO, TrickUISetup> trickDictionary = new();

    private void Awake()
    {
        trickManager = TrickManager.Instance;
        Initialize();
    }

    private void OnEnable()
    {
        trickManager.OnPlayerInput += UpdateUI;
        trickManager.OnTrickStart += ToggleUIPanel;
        trickManager.OnTrickFail += OnTrickEnd;
    }

    private void OnDisable()
    {
        trickManager.OnPlayerInput -= UpdateUI;
        trickManager.OnTrickStart -= ToggleUIPanel;
        trickManager.OnTrickFail -= OnTrickEnd;
    }

    private void Start()
    {
        trickTimeSlider.maxValue = trickManager.MaxTrickTime;
        trickTimeSlider.value = trickManager.MaxTrickTime; // Initialize slider to max value
        trickTime.text = trickManager.MaxTrickTime.ToString("F2"); // Initialize text to max value
    }

    private void Update()
    {
        if (trickManager._trickTimer != null)
        {
            // Update the text and slider with the current timer progress
            float remainingTime = trickManager._trickTimer.Progress * trickManager.MaxTrickTime;
            trickTime.text = ((int)(remainingTime)).ToString("F2");
            trickTimeSlider.value = remainingTime;
        }
    }

    private void Initialize()
    {
        ToggleUIPanel(); // Ensure UI is in the correct state initially
        foreach (TrickSO trick in trickManager.tricks)
        {
            SpawnTrickUIBox(trick);
        }
    }

    public void ToggleUIPanel()
    {
        trickUI.SetActive(!trickUI.activeSelf);
    }

    private void OnTrickEnd()
    {
        ResetUI();
        ToggleUIPanel();
    }

    public void SpawnTrickUIBox(TrickSO trickSO)
    {
        TrickUISetup trickUISetup = Instantiate(trickUIPrefab, trickUIParent.transform);
        trickUISetup.trickSO = trickSO;
        trickDictionary.Add(trickSO, trickUISetup);
        trickUISetup.SetupTrick(trickSO);
    }

    private void UpdateUI()
    {
        foreach (TrickSO trick in trickManager.tricks)
        {
            if (trickManager.possibleTrickCombos.Contains(trick))
                trickDictionary[trick].gameObject.SetActive(true);
            else
                trickDictionary[trick].gameObject.SetActive(false);
        }
    }

    public void ResetUI()
    {
        foreach (TrickSO trick in trickManager.tricks)
        {
            trickDictionary[trick].gameObject.SetActive(true);
        }
    }
}
