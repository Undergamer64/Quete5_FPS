using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi_Behavior : MonoBehaviour
{
    private float height;
    private bool dead = false;
    private Vector3 pos_alive;

    private void Awake()
    {
        height = transform.lossyScale.y;
        pos_alive = transform.position;
    }

    void FixedUpdate()
    {
        if (dead)
        {
            if (pos_alive.y - (height + 1) > transform.position.y)
            {
                transform.position += Vector3.up/2;
            }
            else if (pos_alive.y - (height + 1) < transform.position.y)
            {
                transform.position += Vector3.down/2;
            }
        }
        else
        {
            if (pos_alive.y > transform.position.y)
            {
                transform.position += Vector3.up/2;
            }
            else if (pos_alive.y < transform.position.y)
            {
                transform.position += Vector3.down/2;
            }
        }
    }

    public void death()
    {
        dead = true;
        StartCoroutine(death_timer());
    }

    public IEnumerator death_timer()
    {
        dead = true;
        yield return new WaitForSeconds(2f);
        dead = false;
    }
}
