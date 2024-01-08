using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    GameObject Camera;
    private int sensibility;
    private float rotationX, rotationY;

    private void Awake()
    {
        Camera = transform.GetComponent<GameObject>();
        sensibility = 90;
        rotationX = 0;
        rotationY = 0;
    }

    void Update()
    {
        
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 angle = context.ReadValue<Vector2>()/10;

        rotationX -= angle.y;
        rotationY += angle.x;

        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            transform.GetComponent<Rigidbody>().velocity = context.ReadValue<Vector2>();
        }
    }
}
