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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip cheerSound;
    [SerializeField] private AudioClip booSound;

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
        timer.OnTimerStop += HideCelebration;

    }
    private void OnDisable()
    {
        Event.OnTrickCelebration -= ShowCelebration;
        timer.OnTimerStop -= HideCelebration;

    }
    public void ShowCelebration(PlayerTrickHandler trickHandler)
    {
        Debug.Log("Celebration ...............................");
        result = trickHandler.CurrentResult;
        crowdUIAnimator.MoveAnimate();
        if (result == TrickResult.Complete)
        {
            coolEffect.Play();
            crowdImage.sprite = cheerCrowd;
            AudioManager.Instance.PlayAudio(cheerSound);
            //play cheer sound
        }
        else if (result == TrickResult.Failed)
        {
            crowdImage.sprite = booCrowd;
            AudioManager.Instance.PlayAudio(booSound);
            //play boo sound
        }
        timer.Start();
    }

    public void HideCelebration()
    {
        if (coolEffect.isPlaying)
            coolEffect.Stop();
        crowdUIAnimator.MoveAnimate();
        //timer.Reset();
        Event.OnFinishCelebration?.Invoke();
    }

    private void Update()
    {
        timer.Tick(Time.deltaTime);
    }
}
