using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleUIHandler : MonoBehaviour
{

    [SerializeField] private GameObject PaddleUIPanel;

    [SerializeField] private GameObject RightUI;
    [SerializeField] private GameObject LeftUI;

    public GameEvent Event;

    private void Awake()
    {
        PaddleUIPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ToggleUI);

        Event.OnPaddleLeft += PaddleLeft;
        Event.OnPaddleRight += PaddleRight;
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ToggleUI);

        Event.OnPaddleLeft -= PaddleLeft;
        Event.OnPaddleRight -= PaddleRight;

    }

    private void ToggleUI(GamePhase phase)
    {
        if (phase == GamePhase.Phase1)
        {
            PaddleUIPanel.SetActive(true);
        }
        else
        {
            PaddleUIPanel.SetActive(false);
        }
    }

    private void PaddleLeft()
    {
        RightUI.gameObject.SetActive(true);
        LeftUI.gameObject.SetActive(false);
    }
    private void PaddleRight()
    {
        RightUI.gameObject.SetActive(false);
        LeftUI.gameObject.SetActive(true);
    }

}
