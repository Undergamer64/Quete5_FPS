using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Behavior : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private GameObject Impact;
    [SerializeField] private GameObject Linerenderer;
    [SerializeField] private float recoil;
    [SerializeField] private GameObject next_weapon;
    [SerializeField] private float spread;
    private bool is_aiming = false;
    private bool is_shooting = false;
    private bool is_swapping = false;
    private float temp_sensibility = 0;


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
            if (is_aiming)
            {
                Player.GetComponent<Player_Movement>().rotationX -= recoil/2;
            }
            else
            {
                Player.GetComponent<Player_Movement>().rotationX -= recoil;
            }
            GameObject line = Instantiate(Linerenderer, transform.position, transform.rotation);

            line.GetComponent<LineRenderer>().SetPosition(0, transform.GetChild(4).transform.position);
            RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward + (new Vector3( Random.Range(-spread, spread), Random.Range(-spread/2, spread/2), Random.Range(-spread/2, spread/2))), out hit, 300))
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
            if (spread < 0.1f && !is_aiming)
            {
                spread += 0.01f;
            }
            else if (spread < 0.05f && is_aiming)
            {
                spread += 0.005f;
            }
            yield return new WaitForSeconds(0.15f);
        } while (is_shooting);
        spread = 0f;
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

    private IEnumerator Aiming()
    {
        temp_sensibility = Player.GetComponent<Player_Movement>().sensibility;
        Player.GetComponent<Player_Movement>().sensibility /= 2;
        Crosshair.SetActive(false);
        Vector3 pos1 = transform.localPosition;
        Vector3 pos2 = pos1 - new Vector3(0.7f, -0.10f, 0.5f);
        if (transform.name == "Rifle_00")
        {
            pos2 = pos1 - new Vector3(0.7f, -0.16f, 0.5f);
        }
        float t = 0f;
        while (t < 1f)
        {
            t += 0.2f;
            transform.localPosition = new Vector3(Mathf.Lerp(pos1.x, pos2.x, t), Mathf.Lerp(pos1.y, pos2.y, t), Mathf.Lerp(pos1.z, pos2.z, t));
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator NotAiming()
    {
        Vector3 pos1 = transform.localPosition;
        Vector3 pos2 = new Vector3(0.7f, -0.3f, 1f); ;
        if (transform.name == "Rifle_00")
        {
            pos2 = new Vector3(0.7f, -0.5f, 1f);
        }
        float t = 0f;
        while (t < 1f)
        {
            t += 0.2f;
            transform.localPosition = new Vector3(Mathf.Lerp(pos1.x, pos2.x, t), Mathf.Lerp(pos1.y, pos2.y, t), Mathf.Lerp(pos1.z, pos2.z, t));
            yield return new WaitForSeconds(0.01f);
        }
        Player.GetComponent<Player_Movement>().sensibility = temp_sensibility;
        Crosshair.SetActive(true);
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (this.gameObject.activeSelf)
        {
            if (!is_swapping)
            {
                if (context.started)
                {
                    is_aiming = true;
                    StopCoroutine(NotAiming());
                    StartCoroutine(Aiming());
                }
            }
            if (context.canceled)
            {
                StopCoroutine(Aiming());
                StartCoroutine(NotAiming());
                is_aiming = false;
            }
        }
    }

    public void Switch_weapon(InputAction.CallbackContext context)
    {
        if (context.started && !is_aiming)
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