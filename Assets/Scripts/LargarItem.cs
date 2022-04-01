using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargarItem : MonoBehaviour
{
    public GameObject item;
    public int chance = 1;
    bool criou = false;

    Invector.vHealthController vida;

    void Awake()
    {
        vida = GetComponent<Invector.vHealthController>();
        StaticClass.inimigosVivos++;
        Debug.Log(StaticClass.inimigosVivos);
        criou = false;
    }

    private void Update()
    {
        if(vida.currentHealth <= 0 && criou == false)
        {
            if(Random.Range(1, chance) == 1)
            {
                CriarItem();
            }

            StaticClass.inimigosVivos--;
            Debug.Log(StaticClass.inimigosVivos);
            criou = true;
        }

        if(transform.position.y < -10)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

    public void CriarItem()
    {
        if (item != null)
        {
            var inst = Instantiate(item, transform.position + (transform.up * 2.5f), transform.rotation);
            inst.GetComponent<Rigidbody>().AddForce(transform.up * 300);
        }
    }
}
