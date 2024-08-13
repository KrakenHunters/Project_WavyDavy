using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputDeviceDetector : MonoBehaviour
{
    public delegate void OnInputDeviceChanged(InputDevice device);
    public static event OnInputDeviceChanged InputDeviceChanged;

    private InputDevice currentDevice;

    private void OnEnable()
    {
        InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
    }

    private void OnAnyButtonPress(InputControl control)
    {
        // Get the device used
        InputDevice device = control.device;

        currentDevice = device;
        InputDeviceChanged?.Invoke(device);
    }
}
