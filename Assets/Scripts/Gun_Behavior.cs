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
    private bool is_shooting = false;

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
            StartCoroutine(shoot());
        }
        if (context.canceled)
        {
            is_shooting = false;
        }
    }
}