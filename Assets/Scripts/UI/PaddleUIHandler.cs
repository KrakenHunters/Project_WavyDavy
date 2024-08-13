using UnityEngine;

public class PaddleUIHandler : MonoBehaviour
{

    [SerializeField] private GameObject paddleUIPanel;

    [SerializeField] private GameObject rightUI;
    [SerializeField] private GameObject leftUI;

    [SerializeField] private InputManager inputManager;

   // private bool isController => inputManager.IsController;

    public GameEvent Event;

    private void Awake()
    {
        paddleUIPanel.SetActive(false);
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
            paddleUIPanel.SetActive(true);
        }
        else
        {
            paddleUIPanel.SetActive(false);
        }
    }

    private void PaddleLeft()
    {
        rightUI.GetComponent<UIAnimator>().FadeInAnimate(true);
        leftUI.GetComponent<UIAnimator>().FadeInAnimate(false);
    }
    private void PaddleRight()
    {
        rightUI.GetComponent<UIAnimator>().FadeInAnimate(false);
        leftUI.GetComponent<UIAnimator>().FadeInAnimate(true);
    }

}
