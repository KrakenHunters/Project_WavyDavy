using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField]
    private GameObject popupMenu;
    [SerializeField]
    private GameObject creditsMenu;
    [SerializeField]
    private GameObject controlsMenu;

    [Header("Start Buttons")]
    [SerializeField]
    private Button _loadGame;

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
        //_loadGame.interactable = false;// use this to disable the button 
        //Ui settings
        _startActive = true;
        DisableScreens();
    }


    protected override void DisableScreens()
    {
        base.DisableScreens();
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        popupMenu.SetActive(false);
    }

    public void OnPlay() => SceneManager.LoadScene("02_ForestScene");

    public void OnTogglePopUpMenu() => popupMenu.SetActive(!popupMenu.activeSelf);
    public void OnToggleCredits() => creditsMenu.SetActive(!creditsMenu.activeSelf);
    public void OnToggleControls() => controlsMenu.SetActive(!controlsMenu.activeSelf);


    public void OnToggleMute()
    {
        _isMusted = !_isMusted;
        MuteButtonImage.sprite = _isMusted ? MuteSprite : UnMuteSprite;
        _MasterAudioMixer.SetFloat("Master", _isMusted ? Mathf.Log10(0.001f) * 20 : Mathf.Log10(_maxVolume) * 20);
    }
}
