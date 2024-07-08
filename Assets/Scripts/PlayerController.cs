using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputManager.EnablePlayerPaddle();    
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputManager.EnablePlayerMovement();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputManager.EnablePlayerTrickState();
        }

        Debug.Log(inputManager.Movement);*/

    }
}
