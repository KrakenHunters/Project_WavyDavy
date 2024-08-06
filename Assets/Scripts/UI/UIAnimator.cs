using DG.Tweening;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease = Ease.Linear;

    private Vector2 startPos;
    private RectTransform rectTransform;
    private Tween currentTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    public void MoveAnimate()
    {
        if (currentTween.IsActive())
            return;

        if (rectTransform.anchoredPosition != startPos)
            currentTween = rectTransform.DOAnchorPos(startPos, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOAnchorPos(targetPos, duration).SetEase(ease);

        currentTween.OnComplete(() => currentTween.Kill());
    }

    public void GrowAnimate()
    {
        if (currentTween.IsActive())
            return;

        if (rectTransform.localScale == Vector3.one)
            currentTween = rectTransform.DOScale(Vector3.zero, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOScale(Vector3.one, duration).SetEase(ease);

        currentTween.OnComplete(() => currentTween.Kill());

    }
}
