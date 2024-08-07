using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease = Ease.Linear;

    private Tween currentTween;
    private Vector2 startPos;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private bool isPosChanged;
    private bool isScaleChanged;
    private bool isFadeChanged;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = rectTransform.anchoredPosition;
        isPosChanged = rectTransform.anchoredPosition != startPos;
        isScaleChanged = rectTransform.localScale != Vector3.one;
        isFadeChanged = canvasGroup.alpha != 0;
    }

    public void MoveAnimate()
    {
        if (currentTween.IsActive())
            return;

        if (isPosChanged)
            currentTween = rectTransform.DOAnchorPos(startPos, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOAnchorPos(targetPos, duration).SetEase(ease);

        currentTween.OnComplete(() => 
        {
            currentTween.Kill(); 
            isPosChanged = !isPosChanged;
        });
    }

    public void GrowAnimate()
    {
        if (currentTween.IsActive())
            return;

        if (isScaleChanged)
            currentTween = rectTransform.DOScale(Vector3.zero, duration).SetEase(ease);
        else
            currentTween = rectTransform.DOScale(Vector3.one, duration).SetEase(ease);

        currentTween.OnComplete(() => 
        {
            currentTween.Kill();
            isScaleChanged = !isScaleChanged;
        });

    }

     public void FadeAnimate()
     {
         if (currentTween.IsActive())
             return;

         if (isFadeChanged)
             currentTween = canvasGroup.DOFade(0, duration).SetEase(ease);
         else
             currentTween = canvasGroup.DOFade(1, duration).SetEase(ease);

        currentTween.OnComplete(() => 
         {
             currentTween.Kill();
             isFadeChanged = !isFadeChanged;
         });

     }
}
