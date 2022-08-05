using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargarItem : MonoBehaviour
{
    // Este script faz um personagem ter a chance aleatória de deixar um item ao morrer.

    public GameObject item;
    public int chance = 1;
    bool criou = false;

    Invector.vHealthController vida;

    void Awake()
    {
        vida = GetComponent<Invector.vHealthController>();
        criou = false;

        // O cálculo de chance não funciona se o valor "chance" for menor que 1.
        if(chance < 1)
        {
            chance = 1;
        }
    }

    private void Update()
    {
        if(vida.currentHealth <= 0 && criou == false)
        {
            // Ao morrer, tem a chance aleatória de criar um item.
            // chance 1 = 100%
            // chance 2 = 50%
            // chance 3 = 33%
            // chance 4 = 25%
            // [...]
            // Quanto maior esse valor, menor a chance.

            if(Random.Range(0, chance) == 0)
            {
                CriarItem();
            }

            StaticClass.inimigosVivos--;
            Debug.Log(StaticClass.inimigosVivos);
            criou = true;
        }
    }

    public void CriarItem()
    {
        if (item != null)
        {
            // Criar um item e jogá-lo para cima.
            var inst = Instantiate(item, transform.position + (transform.up * 2.5f), transform.rotation);
            inst.GetComponent<Rigidbody>().AddForce(transform.up * 300);
        }
    }
}
