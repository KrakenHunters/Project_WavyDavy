using Tricks;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
public class TrickUiCelebration : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject crowdPanel;
    [SerializeField] private Image crowdImage;
    [SerializeField] private Sprite cheerCrowd;
    [SerializeField] private Sprite booCrowd;

    [Header("UI Animators")]
    [SerializeField] private UIAnimator crowdUIAnimator;
    [SerializeField] private ParticleSystem coolEffect;

    [Header("Timer")]
    [SerializeField] private float celebrationTime;

    private CountdownTimer timer;
    private TrickResult result;
    public GameEvent Event;

    private void Awake()
    {
        timer = new CountdownTimer(celebrationTime);
    }

    private void OnEnable()
    {
        Event.OnTrickCelebration += ShowCelebration;
    }
    private void OnDisable()
    {
        Event.OnTrickCelebration -= ShowCelebration;
    }
    public void ShowCelebration(PlayerTrickHandler trickHandler)
    {
        result = trickHandler.CurrentResult;
        timer.OnTimerStop += HideCelebration;
        crowdUIAnimator.MoveAnimate();
        if (result == TrickResult.Complete)
        {
            coolEffect.Play();
            crowdImage.sprite = cheerCrowd;
            //play cheer sound
        }
        else if (result == TrickResult.Failed)
        {
            crowdImage.sprite = booCrowd;
            //play boo sound
        }
        timer.Start();
        Debug.Log(timer.Progress);
    }

    public void HideCelebration()
    {
        coolEffect.Stop();

        timer.OnTimerStop -= HideCelebration;
        crowdUIAnimator.MoveAnimate();
        timer.Reset();
        Event.OnFinishCelebration?.Invoke();
        Debug.Log("Celebration Ended");
    }

    private void Update()
    {
        timer.Tick(Time.deltaTime);
    }
}
