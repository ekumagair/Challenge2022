using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectarPerda : MonoBehaviour
{
    Invector.vHealthController vidaJogador;
    bool iniciou = false;
    int cenaAtual;

    public GameObject menuContinuar;
    public GameObject menuPerdeu;

    void Start()
    {
        cenaAtual = SceneManager.GetActiveScene().buildIndex;
        vidaJogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vHealthController>();
        iniciou = false;
    }

    void Update()
    {
        if(vidaJogador.currentHealth <= 0 && iniciou == false)
        {
            StartCoroutine(MudarEstado());
        }

        if (StaticClass.estadoDeJogo == 1)
        {
            menuContinuar.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            menuContinuar.SetActive(false);
        }

        if (StaticClass.estadoDeJogo == -1)
        {
            menuPerdeu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            menuPerdeu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Comma) && StaticClass.debug)
        {
            StaticClass.faseAtual--;
            ReiniciarCena();
        }
        if (Input.GetKeyDown(KeyCode.Period) && StaticClass.debug)
        {
            StaticClass.faseAtual++;
            ReiniciarCena();
        }
        if (Input.GetKeyDown(KeyCode.R) && StaticClass.debug)
        {
            ReiniciarCena();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            VoltarParaTitulo();
        }
    }

    IEnumerator MudarEstado()
    {
        Debug.Log("Morreu");
        iniciou = true;

        yield return new WaitForSeconds(3f);

        StaticClass.estadoDeJogo = -1;
    }

    public void MudarEstadoDeJogo(int e)
    {
        StaticClass.estadoDeJogo = e;
    }

    public void Continuar()
    {
        StaticClass.faseAtual++;
        ReiniciarCena();
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

    public void VoltarParaTitulo()
    {
        if (!Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene("Titulo");
        }
    }
}
