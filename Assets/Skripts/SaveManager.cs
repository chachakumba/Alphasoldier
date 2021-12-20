using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveManager : MonoBehaviour
{
    public static Save save;
    public Save[] allSaves;
    public int currentSaveNum;
    public static SaveManager instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        allSaves = new Save[3];
        Menu.instance.saveManager = this;
        RefreshTimes();
    }
    public void RefreshTimes()
    {
        for (int i = 0; i < allSaves.Length; i++)
        {
            Load(i);
            if (IsSlotOld(i))
            {
                int hours = Mathf.RoundToInt((float)(allSaves[i].playTime / 3600));
                int minutes = Mathf.RoundToInt((float)((allSaves[i].playTime - (allSaves[i].playTime / 3600)) / 60));
                int seconds = Mathf.RoundToInt((float)(allSaves[i].playTime - (allSaves[i].playTime / 60)));
                Menu.instance.saveSlotsNew[i].text = hours.ToString() + "." + minutes.ToString() + "." + seconds.ToString();
                Menu.instance.saveSlotsLoad[i].text = hours.ToString() + "." + minutes.ToString() + "." + seconds.ToString();
            }
            else
            {
                Menu.instance.saveSlotsNew[i].text = "--.--.--";
                Menu.instance.saveSlotsLoad[i].text = "--.--.--";
            }
        }
    }
    public void SetCurrentSave(int slot)
    {
        save = allSaves[slot];
        save.slot = slot;
        currentSaveNum = slot;
        if (save != null)
            Debug.LogWarning("ind: " + save.palyerPosIndex + " health: " + save.playerHealth + " ammo: " + save.playerAmmo);
    }
    public bool IsSlotOld(int slot)
    {
        if(allSaves[slot]!= null)
        //if(allSaves[slot].playTime > 0)
        {
            return true;
        }
        return false;
    }
    public void Create(int slot)
    {
        if (allSaves[slot] == null)
        {
            allSaves[slot] = new Save();
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save" + slot + ".dat");
        bf.Serialize(file, allSaves[slot]);
        file.Close();
        Debug.Log("Save"+slot+" saved");
    }

    public void Load(int slot)
    {
        if (
        File.Exists(Application.persistentDataPath + "/save" + slot + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save" + slot + ".dat", FileMode.Open);
            allSaves[slot] = (Save)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game save" + slot + " loaded!");
        }
        else
        {
            //Debug.LogError("There is no save" + slot + "!");
        }
    }
    public void Erase(int slot)
    {

        if (File.Exists(Application.persistentDataPath
          + "/save" + slot + ".dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/save" + slot + ".dat");
            allSaves[slot] = null;
            Debug.Log("Save" + slot + " reset complete!");
        }
        else
            Debug.LogWarning("No save" + slot + " data to delete.");
    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save" + save.slot + ".dat");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Save " + save.slot + " saved");
    }
}
