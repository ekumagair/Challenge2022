using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;
    public int armaEquipada;
    public bool[] armasDisponiveis;
    public GameObject[] armasModelos;
    public Image[] armasEspacos;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;

    int cenaAtual;

    private void Start()
    {
        cenaAtual = SceneManager.GetActiveScene().buildIndex;
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
        StaticClass.ondasPassadas = 0;
        StaticClass.inimigosVivos = 0;
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
            arma.defaultDamage.damageValue = 20;
            arma.defaultStaminaCost = 5;
        }
        else if (armaEquipada == 1)
        {
            arma.defaultDamage.damageValue = 40;
            arma.defaultStaminaCost = 20;
        }

        for (int i = 0; i < armasModelos.Length; i++)
        {
            if(i != armaEquipada)
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

        if(Input.GetKeyDown(KeyCode.Comma))
        {
            StaticClass.faseAtual--;
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            StaticClass.faseAtual++;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StaticClass.ondasPassadas = 0;
            StaticClass.inimigosVivos = 0;
            SceneManager.LoadScene(cenaAtual);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void CriarSplashEspada()
    {
        Instantiate(splashEspada, transform.position + (transform.forward * 1.5f), transform.rotation);
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
