using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    [SerializeField] Transform newGamePanel;
    [SerializeField] float startnewGamePos = 400;
    [SerializeField] float finalnewGamePos = 0;
    [SerializeField] float newGameSpeed = 0.5f;
    [SerializeField] float newGameDelay = 0.1f;
    [Space]
    [SerializeField] Transform loadGamePanel;
    [SerializeField] float startloadGamePos = 400;
    [SerializeField] float finalloadGamePos = 0;
    [SerializeField] float loadGameSpeed = 0.5f;
    [SerializeField] float loadGameDelay = 0.1f;
    [Space]
    [SerializeField] Transform settingsPanel;
    [SerializeField] float startSettingsPos = -400;
    [SerializeField] float finalSettingsPos = 0;
    [SerializeField] float settingsSpeed = 0.5f;
    [SerializeField] float settingsDelay = 0.1f;
    [Space]
    [SerializeField] Transform changelogPanel;
    [SerializeField] float startChangelogPos = -400;
    [SerializeField] float finalChangelogPos = 0;
    [SerializeField] float changelogSpeed = 0.5f;
    [SerializeField] float changelogDelay = 0.1f;
    public SaveManager saveManager;
    public static Menu instance;
    [Space]
    [SerializeField] Transform surePanel;
    [SerializeField] float startSurePos = -400;
    [SerializeField] float finalSurePos = 0;
    [SerializeField] float sureSpeed = 0.5f;
    [SerializeField] float sureDelay = 0.1f;
    [SerializeField] UnityEngine.UI.Button yesButton;
    [SerializeField] UnityEngine.UI.Button noButton;
    public TextMeshProUGUI[] saveSlotsNew;
    public TextMeshProUGUI[] saveSlotsLoad;
    public string startSceneName;
    [SerializeField] AudioClip UIButtonPressClip;
    [SerializeField] AudioClip panelShower;
    [SerializeField] AudioClip panelCloser;
    [SerializeField] AudioSource buttonAudSource;
    [SerializeField] AudioSource panelsAudSource;
    public void ButtonPressSound()
    {
        buttonAudSource.clip = UIButtonPressClip;
        buttonAudSource.Play();
    }
    private void Awake()
    {
        instance = this;
    }
    public void OpenSurePanel(Action act)
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        surePanel.localPosition = new Vector2(surePanel.localPosition.x, startSurePos);
        surePanel.LeanMoveLocalY(finalSurePos, sureSpeed).setEaseOutExpo().delay = sureDelay;
        yesButton.onClick.AddListener(new UnityEngine.Events.UnityAction(act));
    }
    public void CloseSurePanel()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        surePanel.localPosition = new Vector2(surePanel.localPosition.x, finalSurePos);
        surePanel.LeanMoveLocalY(startSurePos, sureSpeed).setEaseOutExpo().delay = sureDelay;
    }
    public void OpenNewGame()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        newGamePanel.localPosition = new Vector2(startnewGamePos, newGamePanel.localPosition.y);
        newGamePanel.LeanMoveLocalX(finalnewGamePos, newGameSpeed).setEaseOutExpo().delay = newGameDelay;
    }
    public void CloseNewGame()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        newGamePanel.localPosition = new Vector2(finalnewGamePos, newGamePanel.localPosition.y);
        newGamePanel.LeanMoveLocalX(startnewGamePos, newGameSpeed).setEaseOutExpo().delay = newGameDelay;
    }
    public void OpenLoadGame()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        loadGamePanel.localPosition = new Vector2(startloadGamePos, loadGamePanel.localPosition.y);
        loadGamePanel.LeanMoveLocalX(finalloadGamePos, loadGameSpeed).setEaseOutExpo().delay = loadGameDelay;
    }
    public void CloseLoadGame()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        loadGamePanel.localPosition = new Vector2(finalloadGamePos, loadGamePanel.localPosition.y);
        loadGamePanel.LeanMoveLocalX(startloadGamePos, loadGameSpeed).setEaseOutExpo().delay = loadGameDelay;
    }
    public void OpenSettings()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        settingsPanel.localPosition = new Vector2(settingsPanel.localPosition.x, startSettingsPos);
        settingsPanel.LeanMoveLocalY(finalSettingsPos,settingsSpeed).setEaseOutExpo().delay = settingsDelay;
    }
    public void CloseSettings()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        settingsPanel.localPosition = new Vector2(settingsPanel.localPosition.x, finalSettingsPos);
        settingsPanel.LeanMoveLocalY(startSettingsPos, settingsSpeed).setEaseOutExpo().delay = settingsDelay;
    }

    public void OpenChangelog()
    {
        panelsAudSource.clip = panelShower;
        panelsAudSource.Play();
        changelogPanel.localPosition = new Vector2(startChangelogPos, changelogPanel.localPosition.y);
        changelogPanel.LeanMoveLocalX(finalChangelogPos, changelogSpeed).setEaseOutExpo().delay = changelogDelay;
    }
    public void CloseChangelog()
    {
        panelsAudSource.clip = panelCloser;
        panelsAudSource.Play();
        changelogPanel.localPosition = new Vector2(finalChangelogPos, changelogPanel.localPosition.y);
        changelogPanel.LeanMoveLocalX(startChangelogPos, changelogSpeed).setEaseOutExpo().delay = changelogDelay;
    }
    public void LoadNewGameSlot(int slot)
    {
        if (saveManager.IsSlotOld(slot))
        {
            OpenSurePanel(()=> {
                saveManager.Erase(slot);
                saveManager.Create(slot);
                saveManager.SetCurrentSave(slot);
                saveManager.Save();
            });
        }
        else
        {
            saveManager.Erase(slot);
            saveManager.Create(slot);
            saveManager.SetCurrentSave(slot);
            saveManager.Save();
        }
        saveManager.RefreshTimes();
        TransitionManager.instance.ChangeScene(startSceneName);
        //SceneManager.LoadScene(startSceneName);
    }
    public void LoadCreatedGameSlot(int slot)
    {
        if (saveManager.IsSlotOld(slot))
        {
            saveManager.Load(slot);
        }
        else
        {
            saveManager.Create(slot);
        }
        saveManager.SetCurrentSave(slot);
        TransitionManager.instance.ChangeScene(startSceneName);
        //SceneManager.LoadScene(startSceneName);
    }
    public void EraseSlot(int slot)
    {
        if (saveManager.IsSlotOld(slot))
        {
            OpenSurePanel(() =>
            {
                saveManager.Erase(slot);
                saveManager.RefreshTimes();
                CloseSurePanel();
            });
        }
        else
        {
            Debug.Log("Nothing to delete");
        }
    }
    public void Exit()
    {
        OpenSurePanel(() => {
            Application.Quit();
            Debug.Log("Exiting game");
            CloseSurePanel();
        });
    }
}
