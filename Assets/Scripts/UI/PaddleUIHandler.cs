using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleUIHandler : MonoBehaviour
{

    [SerializeField] private GameObject PaddleUIPanel;

    [SerializeField] private GameObject RightAnimator;
    [SerializeField] private GameObject LeftAnimator;

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
        RightAnimator.gameObject.SetActive(true);
        LeftAnimator.gameObject.SetActive(false);
    }
    private void PaddleRight()
    {
        RightAnimator.gameObject.SetActive(false);
        LeftAnimator.gameObject.SetActive(true);
    }

}
