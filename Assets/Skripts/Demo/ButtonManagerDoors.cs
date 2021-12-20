using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagerDoors : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Door[] doors;
    public bool areDoorsOpen = false;
    [SerializeField] SpriteRenderer bulb;
    [SerializeField] Color openColor = Color.green;
    [SerializeField] Color closedColor = Color.red;
    void Start()
    {
        foreach (Button button in buttons) button.OnPress += OnChange;
        if (areDoorsOpen)
        {
            bulb.color = openColor;
            foreach (Door door in doors)
            {
                door.OpenDoor();
            }
        }
        else
        {
            bulb.color = closedColor;
            foreach (Door door in doors)
            {
                door.CloseDoor();
            }
        }
    }

    private void OnChange(bool state)
    {
        bool openDoor = true;
        foreach (Button button in buttons) if (!button.isOn) openDoor = false;

        if(openDoor != areDoorsOpen)
        {
            if (openDoor)
            {
                bulb.color = openColor;
                foreach (Door door in doors)
                {
                    door.OpenDoor();
                }
            }
            else
            {
                bulb.color = closedColor;
                foreach (Door door in doors)
                {
                    door.CloseDoor();
                }
            }
        }
        areDoorsOpen = openDoor;
    }
}
