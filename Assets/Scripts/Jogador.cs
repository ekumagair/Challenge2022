using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;
    public int armaEquipada;
    public bool[] armasDisponiveis;
    public GameObject[] armasModelos;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;

    private void Start()
    {
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && armasDisponiveis[0] == true)
        {
            armaEquipada = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && armasDisponiveis[1] == true)
        {
            armaEquipada = 1;
        }

        if (armaEquipada == 0)
        {
            arma.defaultDamage.damageValue = 10;
            arma.defaultStaminaCost = 5;
        }
        else if (armaEquipada == 1)
        {
            arma.defaultDamage.damageValue = 25;
            arma.defaultStaminaCost = 20;
        }

        for (int i = 0; i < armasModelos.Length; i++)
        {
            if(i != armaEquipada)
            {
                armasModelos[i].SetActive(false);
            }
            else
            {
                armasModelos[i].SetActive(true);
            }
        }
    }

    public void CriarSplashEspada()
    {
        Instantiate(splashEspada, transform.position + transform.forward, transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coletar")
        {
            int da = other.gameObject.GetComponent<ItemColetavel>().desbloquearArma;
            int rv = other.gameObject.GetComponent<ItemColetavel>().recuperarVida;

            if (da >= 0)
            {
                armasDisponiveis[da] = true;
            }
            if(rv > 0)
            {
                vida.AddHealth(rv);
            }
            
            Destroy(other.transform.parent.gameObject);
            Destroy(other.gameObject);
        }
    }
}
