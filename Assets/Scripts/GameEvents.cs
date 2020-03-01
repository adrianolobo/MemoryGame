using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<SimonButton> onButtonPressed;
    public void ButtonPressed(SimonButton buttonPressed)
    {
        onButtonPressed?.Invoke(buttonPressed);
    }

    public event Action onBlockUserInput;
    public void BlockUserInput()
    {
        onBlockUserInput?.Invoke();
    }

    public event Action onAllowUserInput;
    public void AllowUserInput()
    {
        onAllowUserInput?.Invoke();
    }
}
