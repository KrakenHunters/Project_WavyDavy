using UnityEngine;
using UnityEngine.Audio;

public class BGMusicHandler : Singleton<BGMusicHandler>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] phaseSource = new AudioSource[3];

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] phaseClips = new AudioClip[3];

    [Header("Mixer Group")]
    [SerializeField] private AudioMixerGroup _bgMusicGroup;

    public GameEvent Event;

    private GamePhase _currentPhase;

    private void Awake()
    {
        for (int i = 0; i < phaseSource.Length; i++)
        {
            SetAudioGroup(phaseSource[i], phaseClips[i]);
        }
    }

    private void Start()
    {
        foreach (AudioSource source in phaseSource)
        {
            source.Play();
        }
        ChangeState(GamePhase.Phase1);
    }

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(ChangeState);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(ChangeState);
    }

    private void ChangeState(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Phase1:
                phaseSource[0].mute = false;
                phaseSource[1].mute = true;
                phaseSource[2].mute = true;
                break;
            case GamePhase.Phase2:
                phaseSource[0].mute = false;
                phaseSource[1].mute = false;
                phaseSource[2].mute = true;
                break;
            case GamePhase.Phase3:
                phaseSource[0].mute = false;
                phaseSource[1].mute = false;
                phaseSource[2].mute = false;
                break;
            default:
                break;
        }
    }

    private void SetAudioGroup(AudioSource source, AudioClip clip)
    {
        source.outputAudioMixerGroup = _bgMusicGroup;
        source.loop = true;
        source.clip = clip;
    }
}
