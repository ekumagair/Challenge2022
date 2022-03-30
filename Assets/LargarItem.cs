using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargarItem : MonoBehaviour
{
    public GameObject item;
    bool criou = false;

    Invector.vHealthController vida;

    void Awake()
    {
        vida = GetComponent<Invector.vHealthController>();
        criou = false;
    }

    private void Update()
    {
        if(vida.currentHealth <= 0 && criou == false)
        {
            CriarItem();
            criou = true;
        }
    }

    public void CriarItem()
    {
        var inst = Instantiate(item, transform.position + (transform.up * 2), transform.rotation);
        inst.GetComponent<Rigidbody>().AddForce(transform.up * 300);
    }
}
