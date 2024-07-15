using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease = Ease.Linear;

    private Vector3 startPos;
    private RectTransform rectTransform;
    private Tween currentTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    public void MoveAnimate()
    {
        if (currentTween != null) currentTween.Kill();

        if (rectTransform.anchoredPosition == targetPos)
            currentTween = rectTransform.DOAnchorPos(startPos, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOAnchorPos(targetPos, duration).SetEase(ease);
    }

    public void GrowAnimate()
    {
       if (currentTween != null) currentTween.Kill();

        if (rectTransform.localScale == Vector3.one)
            currentTween = rectTransform.DOScale(Vector3.zero, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOScale(Vector3.one, duration).SetEase(ease);
    }
}
