using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSceneManager : MonoBehaviour
{
    public static TransitionSceneManager instance;
    public CanvasGroup mainCanvasGroup;
    private void Awake()
    {
        instance = this;
    }
}
