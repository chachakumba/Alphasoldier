using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Win();
    }
    public void Win()
    {
        CanvasManager.instance.OpenWin();
        Time.timeScale = 0;
    }
}
