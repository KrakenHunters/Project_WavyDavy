using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameEvent Event;

    [SerializeField] private GameObject timesUpText;
    [SerializeField] private GameObject fadeInImage;


    private void OnEnable()
    {
        Event.OnGameEnd += EndGameUI;
    }

    private void OnDisable()
    {
        Event.OnGameEnd -= EndGameUI;
    }

    private void EndGameUI()
    {

        timesUpText.GetComponent<UIAnimator>().MoveAnimate();
        fadeInImage.GetComponent<UIAnimator>().FadeAnimate();
        fadeInImage.GetComponent <UIAnimator>().OnAnimateFinished.AddListener(SceneTransition);
    }

    private void SceneTransition()
    {
        SceneManager.LoadScene(2);
    }

}
