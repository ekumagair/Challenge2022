using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;
    public GameObject splashMachado;
    public int armaEquipada;
    public bool[] armasDisponiveis;
    public GameObject[] armasModelos;
    public Image[] armasEspacos;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;
    public Invector.vCamera.vThirdPersonCamera cameraThirdPerson;

    private void Start()
    {
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
    }

    private void Update()
    {
        if (StaticClass.estadoDeJogo == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && armasDisponiveis[0] == true)
            {
                armaEquipada = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && armasDisponiveis[1] == true)
            {
                armaEquipada = 1;
            }

            if (armaEquipada == 0)
            {
                arma.defaultDamage.damageValue = 20;
                arma.defaultStaminaCost = 5;
            }
            else if (armaEquipada == 1)
            {
                arma.defaultDamage.damageValue = 45;
                arma.defaultStaminaCost = 25;
            }

            for (int i = 0; i < armasModelos.Length; i++)
            {
                if (i != armaEquipada)
                {
                    armasModelos[i].SetActive(false);
                    armasEspacos[i].color = new Color32(150, 150, 150, 200);
                }
                else
                {
                    armasModelos[i].SetActive(true);
                    armasEspacos[i].color = new Color32(255, 255, 0, 200);
                }
            }
        }
    }

    public void CriarSplashEspada()
    {
        if (armaEquipada == 0)
        {
            Instantiate(splashEspada, transform.position + (transform.forward * 1.5f), transform.rotation);
        }
        else if (armaEquipada == 1)
        {
            Instantiate(splashMachado, transform.position + (transform.forward * 1.75f), transform.rotation);
        }
    }

    public void DestruirCamera()
    {
        Destroy(cameraThirdPerson);
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
