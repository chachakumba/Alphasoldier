using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
public class Settings : MonoBehaviour
{
    public Config config;
    List<Resolution> resolutions = new List<Resolution>();
    [SerializeField] List<int> refreshRates = new List<int>();

    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [SerializeField] TMP_Dropdown refreshRateDropdown;
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullscreenToggle;

    [SerializeField] UnityEngine.Audio.AudioMixer masterMixer;
    [SerializeField] TextMeshProUGUI masterVol;
    [SerializeField] Slider masterSlider;
    [SerializeField] TextMeshProUGUI musicVol;
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI soundsVol;
    [SerializeField] Slider soundsSlider;
    [SerializeField] AudioClip panelShower;
    public void Start()
    {
        RevertChanges();

        resolutionsDropdown.ClearOptions();
        List<string> resolutionsStr = new List<string>();
        Resolution[] allRes = Screen.resolutions;
        foreach(Resolution newRsl in allRes)
        {
            bool newToArray = true;
            foreach(Resolution rsl in resolutions)
            {
                if (rsl.width == newRsl.width && rsl.height == newRsl.height) newToArray = false;
            }
            if (newToArray) resolutions.Add(newRsl);
        }
        int currentRsl = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionsStr.Add(option);
            if (resolutions[i].width == config.width && resolutions[i].height == config.height)
                currentRsl = i;
        }
        resolutionsDropdown.AddOptions(resolutionsStr);
        if(config.width == 0 || config.height == 0)
        {
            config.width = resolutions[resolutions.Count - 1].width;
            config.height = resolutions[resolutions.Count - 1].height;
            currentRsl = resolutions.Count - 1;
        }
        resolutionsDropdown.value = currentRsl;
        resolutionsDropdown.RefreshShownValue();


        refreshRateDropdown.ClearOptions();
        List<string> refreshRatesStr = new List<string>();
        foreach (Resolution newRsl in allRes)
        {
            bool newToArray = true;
            foreach (int refreshRate in refreshRates)
            {
                if (refreshRate == newRsl.refreshRate) newToArray = false;
            }
            if (newToArray) refreshRates.Add(newRsl.refreshRate);
        }
        refreshRates.Sort();
        int currentRef = 0;
        for (int i = 0; i < refreshRates.Count; i++)
        {
            string option = refreshRates[i].ToString();
            refreshRatesStr.Add(option);
            if (Screen.currentResolution.refreshRate == refreshRates[i])
                currentRef = i;
        }
        refreshRateDropdown.AddOptions(refreshRatesStr);
        if(currentRef == 0)
        {
            currentRef = refreshRates[refreshRates.Count - 1];
        }
        refreshRateDropdown.value = currentRef;
        refreshRateDropdown.RefreshShownValue();

        qualityDropdown.ClearOptions();
        List<string> qualityStr = new List<string>();
        int currentQual = 0;
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            string option = QualitySettings.names[i];
            qualityStr.Add(option);
            if (QualitySettings.GetQualityLevel() == i)
                currentQual = i;
        }
        qualityDropdown.AddOptions(qualityStr);
        qualityDropdown.value = currentQual;
        qualityDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreen;


        UpdateAudioTexts();

    }
    public void SetVolumeMaster()
    {
        float value = masterSlider.value;

        if (value <= 0) value = 0.0001f;
        masterMixer.SetFloat("master", Mathf.Log10(value) * 20);

        config.masterVol = value;

        masterVol.text = "Master: " + Mathf.RoundToInt(value * 100);
    }
    public void SetVolumeMusic()
    {
        float value = musicSlider.value;

        if (value <= 0) value = 0.0001f;
        masterMixer.SetFloat("music", Mathf.Log10(value) * 20);


        config.musicVol = value;

        musicVol.text = "Music: " + Mathf.RoundToInt(value * 100);
    }
    public void SetVolumeSounds()
    {
        float value = soundsSlider.value;

        if (value <= 0) value = 0.0001f;
        masterMixer.SetFloat("sounds", Mathf.Log10(value) * 20);

        config.soundsVol = value;

        soundsVol.text = "Sounds: " + Mathf.RoundToInt(value * 100);
    }
    public void UpdateAudioTexts()
    {
        masterSlider.value = config.masterVol;
        masterVol.text = "Master: " + Mathf.RoundToInt(config.masterVol * 100);
        musicSlider.value = config.musicVol;
        musicVol.text = "Music: " + Mathf.RoundToInt(config.musicVol * 100);
        soundsSlider.value = config.soundsVol;
        soundsVol.text = "Sounds: " + Mathf.RoundToInt(config.soundsVol * 100);
    }
    public void SetQuality()
    {
        config.quality = qualityDropdown.value;
        //Debug.Log("New res quality: " + config.quality +" name: " + QualitySettings.names[config.quality]);
        QualitySettings.SetQualityLevel(config.quality);
    }
    public void SetResolution()
    {
        int r = resolutionsDropdown.value;
        config.width = resolutions[r].width;
        config.height = resolutions[r].height;
        //Debug.Log("New res x: " + config.width +" y: " + config.height);
        Screen.SetResolution(config.width, config.height, config.isFullscreen, config.refreshRate);
    }
    public void SetFramerate()
    {
        int r = refreshRateDropdown.value;
        config.refreshRate = refreshRates[r];
        //Debug.Log("New refresh rate: " + config.refreshRate);
        Application.targetFrameRate = config.refreshRate;
    }
    public void ChangeFullscreen()
    {
        bool r  = fullscreenToggle.isOn;
        config.isFullscreen = r;
        //Debug.Log("Fullscreen: " + config.isFullscreen);
        Screen.fullScreen = config.isFullscreen;
    }
    public void RevertChanges()
    {
        LoadConfig();
        QualitySettings.SetQualityLevel(config.quality);
        Screen.SetResolution(config.width, config.height, config.isFullscreen, config.refreshRate);
        Application.targetFrameRate = config.refreshRate;
        Screen.fullScreen = config.isFullscreen;

        masterMixer.SetFloat("master", Mathf.Log10(config.masterVol) * 20);
        masterMixer.SetFloat("music", Mathf.Log10(config.musicVol) * 20);
        masterMixer.SetFloat("sounds", Mathf.Log10(config.soundsVol) * 20);
        UpdateAudioTexts();
    }
    void LoadConfig()
    {
        if (
        File.Exists(Application.persistentDataPath + "/Config.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Config.dat", FileMode.Open);
            config = (Config)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game config loaded!");
        }
        else
        {
            Debug.LogWarning("There is no save config!");
            config = new Config();
        }
    }
    public void EraseData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/Config.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/Config.dat");
            config = new Config();
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogWarning("No save data to delete.");
        RevertChanges();
    }
    public void SaveConfig()
    {
        if (config == null)
        {
            LoadConfig();
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Config.dat");
        bf.Serialize(file, config);
        file.Close();
        Debug.Log("Config saved");
    }
}
[System.Serializable]
public class Config
{
    public bool isFullscreen = true;
    //public Resolution resolution;
    public int width;
    public int height;
    public int refreshRate;
    public int quality;
    public float masterVol = 1;
    public float musicVol = 1;
    public float soundsVol = 1;

    public Config()
    {
        //isFullscreen = Screen.fullScreen;
        //resolution = Screen.currentResolution;
        //quality = QualitySettings.GetQualityLevel();
    }
}