using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerUIHandler : MonoBehaviour
{
    [SerializeField] private Slider gameSlider;
    [SerializeField] private TextMeshProUGUI gameTimerText; 
    [SerializeField] private GameObject TimerUIPanel;

    public GameEvent Event;

    private void Awake()
    {
        TimerUIPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Event.OnSetGameTimer += SetMaxTime;
        Event.OnUpdateGameTimer += UpdateUI;
    }
    private void OnDisable()
    {
        Event.OnSetGameTimer -= SetMaxTime;
        Event.OnUpdateGameTimer -= UpdateUI;
    }

    private void SetMaxTime(float val)
    {
        gameSlider.maxValue = val;
        TimerUIPanel.SetActive(true);
    }

    private void UpdateUI(float time)
    {
        gameSlider.value = time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        gameTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00"); ;
    }
}
