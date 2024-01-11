using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Behavior : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject Impact;
    [SerializeField] private GameObject Linerenderer;
    [SerializeField] private float recoil;
    [SerializeField] private GameObject next_weapon;
    private bool is_shooting = false;
    private bool is_swapping = false;


    private void Update()
    {
        
    }

    private IEnumerator despawn_bullet(GameObject line)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(line);
    }

    private IEnumerator shoot()
    {
        do
        {
            if (is_swapping)
            {
                break;
            }
            Player.GetComponent<Player_Movement>().rotationX -= recoil;
            GameObject line = Instantiate(Linerenderer, transform.position, transform.rotation);

            line.GetComponent<LineRenderer>().SetPosition(0, transform.GetChild(4).transform.position);
            RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, 300))
            {
                line.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                GameObject bulletimpact = Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
                if (hit.collider.tag == "Ennemi")
                {
                    hit.collider.gameObject.GetComponent<Ennemi_Behavior>().death();
                }
            }
            else
            {
                line.GetComponent<LineRenderer>().SetPosition(1, Camera.transform.position + 300 * Camera.transform.forward);
            }
            StartCoroutine(despawn_bullet(line));
            yield return new WaitForSeconds(0.15f);
        } while (is_shooting);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (transform.name == "Rifle_00")
            {
                is_shooting = true;
            }
            if (!is_swapping)
            {
                StartCoroutine(shoot());
            }
        }
        if (context.canceled)
        {
            is_shooting = false;
        }
    }

    public void Switch_weapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (this.gameObject.activeSelf)
            {
                is_swapping = true;
                StartCoroutine(swap());
            }
        }
    }

    private IEnumerator swap()
    {
        float t = 0f;
        while (t < 1)
        {
            t += 0.1f;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(1f, -1.5f, t));
            yield return new WaitForFixedUpdate();
        }
        next_weapon.SetActive(true);
        Player.GetComponent<Player_Movement>().ActiveGun = next_weapon;
        t = 0f;
        while (t < 1)
        {
            t += 0.1f;
            next_weapon.transform.localPosition = new Vector3(next_weapon.transform.localPosition.x, next_weapon.transform.localPosition.y, Mathf.Lerp(-1.5f, 1f, t));
            yield return new WaitForFixedUpdate();
        }
        this.gameObject.SetActive(false);
        is_swapping = false;
    }

}