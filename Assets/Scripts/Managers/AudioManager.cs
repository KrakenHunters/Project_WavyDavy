using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup bgMusicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource speaker;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip timeoutClip;

    private List<AudioSource> _audioSources = new();

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            _audioSources.Add(source);
        }
    }

    public void PlayAudio(AudioClip clip, bool isBgMusic = false)
    {
        AudioSource audioSource = GetAvailableAudioSource();

        if (isBgMusic) StopAllAudio();

        if (audioSource)
        {
            audioSource.outputAudioMixerGroup = isBgMusic ? bgMusicGroup : sfxGroup;
            audioSource.loop = isBgMusic;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No available audio sources");
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        AudioSource newAudioSource = Instantiate(speaker,transform);
        return newAudioSource;
    }

    public void StopBGAudio()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
           if(audioSource.outputAudioMixerGroup == bgMusicGroup)
            {
                audioSource.Stop();
            }
        }
    }

    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
    }

    public void PlayTimeoutSound()
    {
        PlayAudio(timeoutClip);
    }

    public void MuteAudio(bool isMuted,float volume)
    {
        mixer.SetFloat("Master", isMuted ? Mathf.Log10(0.001f) * 20 : Mathf.Log10(volume) * 20);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
