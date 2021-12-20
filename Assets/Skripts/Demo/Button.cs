using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ButtonEvent : UnityEvent<bool>
{
}

public class Button : MonoBehaviour
{
    public bool isOn = false;
    bool entered = false;
    Collider2D col;
    PlayerControls controls;
    public delegate void ButtonState(bool state);
    public event ButtonState OnPress;
    public ButtonEvent OnPressUn;
    [SerializeField] SpriteRenderer bulb;
    [SerializeField] Color openColor = Color.green;
    [SerializeField] Color closedColor = Color.red;
    public bool oneTimePressable = true;
    [SerializeField] AudioClip buttonPress;
    AudioSource audiosSource;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        controls = new PlayerControls();
        audiosSource = GetComponent<AudioSource>();
        audiosSource.clip = buttonPress;
        controls.Player.Use.performed += ctx => Press();
        if (isOn) bulb.color = openColor;
        else bulb.color = closedColor;
    }
    public void Press()
    {
        if (entered)
        {
            audiosSource.Play();
            isOn = !isOn;
            OnPress?.Invoke(isOn);
            OnPressUn.Invoke(isOn);
            if (isOn) bulb.color = openColor;
            else bulb.color = closedColor;
            if (oneTimePressable) enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        entered = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (col.Cast(Vector2.zero, new RaycastHit2D[0]) == 0) entered = false;
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}