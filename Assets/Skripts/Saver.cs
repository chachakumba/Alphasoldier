using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public int posIndex;
    public void Save()
    {
        if (SaveManager.save == null)
        {
            SaveManager.save = new Save();
            SaveManager.save.slot = SaveManager.instance.currentSaveNum;
        }
        SaveManager.save.playerHealth = Manager.instance.player.health;
        SaveManager.save.playerAmmo = Manager.instance.player.weapon.currentAmmo;
        SaveManager.save.palyerPosIndex = posIndex;
        SaveManager.instance.Save();
        Debug.LogWarning("Saved at posInd: " + SaveManager.save.palyerPosIndex + " health: " + SaveManager.save.playerHealth + " ammo: " + SaveManager.save.playerAmmo);
    }
}
