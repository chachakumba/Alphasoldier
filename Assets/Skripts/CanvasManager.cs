using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] Transform pausePanel;
    [SerializeField] float startpausePos = 400;
    [SerializeField] float finalpausePos = 0;
    [SerializeField] float pauseSpeed = 0.5f;
    [SerializeField] float pauseDelay = 0.1f;
    bool pauseOpen = false;
    [Space]
    [SerializeField] Transform winPanel;
    [SerializeField] float startwinPos = -400;
    [SerializeField] float finalwinPos = 0;
    [SerializeField] float winSpeed = 0.5f;
    [SerializeField] float winDealy = 0.1f;
    bool winOpen = false;
    [Space]
    [SerializeField] Transform settingsPanel;
    [SerializeField] float startSettingsPos = -400;
    [SerializeField] float finalSettingsPos = 0;
    [SerializeField] float settingsSpeed = 0.5f;
    [SerializeField] float settingsDelay = 0.1f;
    bool settingsOpen = false;
    [Space]
    [SerializeField] Transform deathPanel;
    [SerializeField] float startDeathPos = -400;
    [SerializeField] float finalDeathPos = 0;
    [SerializeField] float deathSpeed = 0.5f;
    [SerializeField] float deathDelay = 0.1f;
    bool deathOpen = false;
    [SerializeField] CanvasGroup deathBackPanel;
    [SerializeField] float deathBackSpeed = 0.5f;
    [SerializeField] float deathBackAlpha = 0.5f;
    [SerializeField] string menuSceneName = "MainMenu";
    [SerializeField] string thisSceneName = "Demo";
    [SerializeField] AudioClip panelShower;
    [SerializeField] AudioClip panelCloser;
    [SerializeField] AudioClip UIButtonPressClip;
    [SerializeField] AudioSource buttonAudSource;
    [SerializeField] AudioSource panelsAudSource;

    PlayerControls controls;
    private void Awake()
    {
        instance = this;
        controls = new PlayerControls();
        controls.Menu.Pause.performed += ctx => Pause();
    }
    public void ButtonPressPlaySound()
    {
        buttonAudSource.clip = UIButtonPressClip;
        buttonAudSource.Play();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        TransitionManager.instance.ChangeScene(menuSceneName);
        //SceneManager.LoadScene(menuSceneName);
    }
    private void Pause()
    {
        if (!winOpen && !deathOpen && !settingsOpen)
            if (pauseOpen)
            {
                ClosePause();
            }
            else
            {
                OpenPause();
            }
    }
    public void OpenDeath()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        Manager.instance.DisablePlayerControls();
        Time.timeScale = 0;
        deathOpen = true;
        deathPanel.localPosition = new Vector2(pausePanel.localPosition.x, startpausePos);
        deathPanel.LeanMoveLocalY(finalpausePos, deathSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = deathDelay;
        deathBackPanel.LeanAlpha(deathBackAlpha, deathBackSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = deathSpeed;
    }
    public void RestartScene()
    {
        Time.timeScale = 1;
        //TransitionManager.instance.ChangeScene(thisSceneName);
        SceneManager.LoadScene(thisSceneName);
    }
    public void OpenPause()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        Manager.instance.DisablePlayerControls();
        Time.timeScale = 0;
        pauseOpen = true;
        pausePanel.localPosition = new Vector2(pausePanel.localPosition.x, startpausePos);
        pausePanel.LeanMoveLocalY(finalpausePos, pauseSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = pauseDelay;
    }
    public void ClosePause()
    {
        Debug.LogWarning("Closing Pause");
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        Manager.instance.EnablePlayerControls();
        Time.timeScale = 1;
        pauseOpen = false;
        pausePanel.localPosition = new Vector2(pausePanel.localPosition.x, finalpausePos);
        pausePanel.LeanMoveLocalY(startpausePos, pauseSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = pauseDelay;
    }
    public void OpenWin()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        winOpen = true;
        Manager.instance.DisablePlayerControls();
        winPanel.localPosition = new Vector2(winPanel.localPosition.x, startwinPos);
        winPanel.LeanMoveLocalY(finalwinPos, winSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = winDealy;
    }
    public void CloseWin()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        winOpen = false;
        Manager.instance.EnablePlayerControls();
        winPanel.localPosition = new Vector2(winPanel.localPosition.x, finalwinPos);
        winPanel.LeanMoveLocalY(startwinPos, winSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = winDealy;
    }
    public void OpenSettings()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        settingsOpen = true;
        settingsPanel.localPosition = new Vector2(settingsPanel.localPosition.x, startSettingsPos);
        settingsPanel.LeanMoveLocalY(finalSettingsPos, settingsSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = settingsDelay;
    }
    public void CloseSettings()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        settingsOpen = false;
        settingsPanel.localPosition = new Vector2(settingsPanel.localPosition.x, finalSettingsPos);
        settingsPanel.LeanMoveLocalY(startSettingsPos, settingsSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = settingsDelay;
    }
}
