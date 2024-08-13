using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputDeviceDetector : MonoBehaviour
{
    public delegate void OnInputDeviceChanged(InputDevice device);
    public static event OnInputDeviceChanged InputDeviceChanged;

    private void OnEnable()
    {
        InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
    }

    private void OnAnyButtonPress(InputControl control)
    {
        InputDeviceChanged?.Invoke(control.device);
    }
}
