using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public GameObject Player;
    GameObject Camera;

    private void Awake()
    {
        Camera = Player.GetComponent<GameObject>();
    }

    void Update()
    {
        
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 angle = context.ReadValue<Vector2>();
    }

}
