using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagerGround : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject[] walls;
    public bool areDoorsOpen = false;
    [SerializeField] SpriteRenderer bulb;
    [SerializeField] Color openColor = Color.green;
    [SerializeField] Color closedColor = Color.red;
    void Awake()
    {
        foreach (Button button in buttons) button.OnPress += OnChange;
        if (areDoorsOpen)
        {
            bulb.color = openColor;
            foreach (GameObject wall in walls)
            {
                wall.SetActive(false);
            }
        }
        else
        {
            bulb.color = closedColor;
            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }
        }
    }

    private void OnChange(bool state)
    {
        bool openDoor = true;
        foreach (Button button in buttons) if (!button.isOn) openDoor = false;

        if (openDoor != areDoorsOpen)
        {
            if (openDoor)
            {
                bulb.color = openColor;
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(false);
                }
            }
            else
            {
                bulb.color = closedColor;
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(true);
                }
            }
        }
        areDoorsOpen = openDoor;
    }
}
