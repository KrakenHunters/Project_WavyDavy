using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    public UnityEvent OnPause;
    public UnityEvent OnResume;
    private bool _isPaused;
    private float _initialTimeScale;
    private void Awake()
    {
        _isPaused = false;
       _startActive = false;
       
    }

    public void OnTogglePauseMenu()
    {
        _isPaused = !_isPaused;
        _currentMenu.gameObject.SetActive(_isPaused); 

        if (_isPaused)
        {
            _initialTimeScale = Time.timeScale;
            Time.timeScale = 0; 
            OnPause.Invoke(); 
        }
        else
        {
            Time.timeScale = _initialTimeScale;
            OnResume.Invoke(); 
        }

    }


    public void OnLoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("00_MainMenu");
    }

    public void OnReturnToShop()
    {
        SceneManager.LoadScene("01_Shop");
    }


}
