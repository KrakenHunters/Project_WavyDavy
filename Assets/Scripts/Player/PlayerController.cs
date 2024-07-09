using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    private float speed;

    public Vector2 MovementDirection { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.EnablePlayerMovement(); //Move this

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

        HandleCharacterMovement();
    }

    //Move this to movement state!
    void HandleCharacterMovement() 
    {
        MovementDirection = inputManager.Movement.normalized;
        transform.position = transform.position + (Vector3)MovementDirection * speed * Time.deltaTime;

    }




}
