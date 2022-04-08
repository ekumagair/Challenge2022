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

    public GameObject menuContinuar;
    public GameObject menuPerdeu;

    Invector.vMelee.vMeleeManager arma;
    Invector.vHealthController vida;

    int cenaAtual;

    private void Start()
    {
        cenaAtual = SceneManager.GetActiveScene().buildIndex;
        arma = GetComponent<Invector.vMelee.vMeleeManager>();
        vida = GetComponent<Invector.vHealthController>();
        ReiniciarVariaveis();
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
                arma.defaultDamage.damageValue = 40;
                arma.defaultStaminaCost = 20;
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
        
        if(StaticClass.estadoDeJogo == 1)
        {
            menuContinuar.SetActive(true);
        }
        else
        {
            menuContinuar.SetActive(false);
        }

        if (StaticClass.estadoDeJogo == -1)
        {
            menuPerdeu.SetActive(true);
        }
        else
        {
            menuPerdeu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            StaticClass.faseAtual--;
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            StaticClass.faseAtual++;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarCena();
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

    public void MudarEstadoDeJogo(int e)
    {
        StaticClass.estadoDeJogo = e;

        if(StaticClass.estadoDeJogo == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Continuar()
    {
        StaticClass.faseAtual++;
        ReiniciarVariaveis();
        SceneManager.LoadScene(cenaAtual);
    }

    public void ReiniciarVariaveis()
    {
        StaticClass.estadoDeJogo = 0;
        StaticClass.ondasPassadas = 0;
        StaticClass.inimigosVivos = 0;
    }

    public void ReiniciarCena()
    {
        ReiniciarVariaveis();
        SceneManager.LoadScene(cenaAtual);
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
