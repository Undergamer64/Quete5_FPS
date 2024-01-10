using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Model;
    public LayerMask LayerMask;
    public GameObject ActiveGun;

    [SerializeField] private float movespeed;
    [SerializeField] private float JumpPower;
    private int sensibility;
    private float rotationX, rotationY;
    private bool Grounded = false;
    [SerializeField] private float gravity;
    private Vector3 VerticalVelocity = Vector3.zero;
    private Vector3 HorizontalVelocity;
    private bool mouselock = true;

    private void Awake()
    {
        sensibility = 90;
        rotationX = 0;
        rotationY = 0;
    }

    void FixedUpdate()
    {
        if (mouselock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Grounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        transform.GetComponent<CharacterController>().Move((HorizontalVelocity.x * Model.transform.right + HorizontalVelocity.y * Model.transform.forward) * Time.fixedDeltaTime);
        
        VerticalVelocity += Vector3.down * gravity * Time.fixedDeltaTime;

        transform.GetComponent<CharacterController>().Move(VerticalVelocity * Time.fixedDeltaTime);
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (mouselock)
        {
            Vector2 angle = context.ReadValue<Vector2>() / 900 * sensibility;

            rotationX -= angle.y;
            rotationY += angle.x;

            rotationX = Mathf.Clamp(rotationX, -80f, 80f);

            Camera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            Model.transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        HorizontalVelocity = new Vector3(context.ReadValue<Vector2>().x * movespeed, context.ReadValue<Vector2>().y * movespeed, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (Grounded)
        {
            if (context.started)
            {
                VerticalVelocity = Vector3.up * JumpPower;
            }
        }
        if (context.canceled)
        {
            if (VerticalVelocity.y >= 0)
            {
                VerticalVelocity.y = VerticalVelocity.y * 0.50f;
            }
        }
    }

    public void LockMouse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            mouselock = false;
        }
        if (context.canceled)
        {
            mouselock = true;
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        ActiveGun.GetComponent<Gun_Behavior>().Shoot(context);
    }


}
