using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Model;
    public LayerMask LayerMask;
    public GameObject ActiveGun;

    [SerializeField] private float movespeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private GameObject Slider;
    [SerializeField] private GameObject text;
    public float sensibility;
    public float rotationX, rotationY;
    public bool Grounded = false;
    [SerializeField] private float gravity;
    private Vector3 VerticalVelocity = Vector3.zero;
    private Vector3 HorizontalVelocity;
    private bool mouselock = true;

    private void Awake()
    {
        sensibility = 80;
        rotationX = 0;
        rotationY = 90;
    }

    private void Start()
    {
        Slider.GetComponent<Slider>().onValueChanged.AddListener((v) =>
        {
            sensibility = v;
        });
    }

    void FixedUpdate()
    {
        text.GetComponent<TextMeshProUGUI>().text = sensibility.ToString();

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

        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        Camera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Model.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (mouselock)
        {
            Vector2 angle = context.ReadValue<Vector2>() / 900 * sensibility;

            rotationX -= angle.y;
            rotationY += angle.x;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (mouselock)
        {
            HorizontalVelocity = new Vector3(context.ReadValue<Vector2>().x * movespeed, context.ReadValue<Vector2>().y * movespeed, 0);
        }
        else
        {
            HorizontalVelocity = Vector3.zero;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (mouselock)
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
        if (mouselock)
        {
            ActiveGun.GetComponent<Gun_Behavior>().Shoot(context);
        }
    }

}
