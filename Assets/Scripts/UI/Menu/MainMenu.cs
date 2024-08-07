using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField]
    private GameObject exhibitionMenu;
    [SerializeField]
    private GameObject creditsMenu;
    [SerializeField]
    private GameObject controlsMenu;

    [Header("Mute button")]
    [SerializeField] private Image MuteButtonImage;
    [SerializeField] private Sprite MuteSprite;
    [SerializeField] private Sprite UnMuteSprite;
    
    [Header("Audio")]
    [SerializeField]
    private AudioClip menuClip;
    [SerializeField]
    private AudioClip buttonClip;

    [Header("Audio Souce")]
    [SerializeField, Tooltip("Audio Mixer form the Assets folder")]
    private AudioMixer _MasterAudioMixer;

    
    private float _maxVolume = 1f;

    private void Start()
    {
        _startActive = true;
       AudioManager.Instance.PlayAudio(menuClip,true);
    }

    public void OnPlay_01() 
    {
        AudioManager.Instance.PlayAudio(buttonClip); 
        SceneManager.LoadScene("01_Career");
    } 
    public void OnPlay_02() 
    {
        AudioManager.Instance.PlayAudio(buttonClip);
        SceneManager.LoadScene("02_Exhibition");
    } 


    public void OnToggleMute()
    {
        _isMusted = !_isMusted;
        MuteButtonImage.sprite = _isMusted ? MuteSprite : UnMuteSprite;
        _MasterAudioMixer.SetFloat("Master", _isMusted ? Mathf.Log10(0.001f) * 20 : Mathf.Log10(_maxVolume) * 20);
    }
}
