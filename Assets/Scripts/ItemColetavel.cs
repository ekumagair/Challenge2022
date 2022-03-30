using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public int desbloquearArma = -1;
    public int recuperarVida = 0;

    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if(rb.velocity.y != 0)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
}
