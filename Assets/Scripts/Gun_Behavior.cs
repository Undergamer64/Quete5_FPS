using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Behavior : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    private LineRenderer linerenderer;

    private void Update()
    {
        linerenderer = GetComponent<LineRenderer>();
    }

    private IEnumerator despawn_bullet()
    {
        yield return new WaitForSeconds(0.1f);
        linerenderer.enabled = false;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            linerenderer.enabled = true;
            RaycastHit hit;
            linerenderer.SetPosition(0, transform.GetChild(4).transform.position);
            
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, 300))
            {
                linerenderer.SetPosition(1, hit.point);
            }
            else
            {
                linerenderer.SetPosition(1, Camera.transform.position + 300 * Camera.transform.forward);
            }
            StartCoroutine(despawn_bullet());
        }
    }
}