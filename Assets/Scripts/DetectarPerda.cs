using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectarPerda : MonoBehaviour
{
    Invector.vHealthController vidaJogador;
    bool perdeu = false;
    bool venceu = false;
    int cenaAtual;

    public GameObject menuContinuar;
    public GameObject menuPerdeu;
    public GameObject menuTitulo;
    public GameObject menuTextoExtra;

    Text menuTituloText;
    Text menuTextoExtraText;

    void Start()
    {
        cenaAtual = SceneManager.GetActiveScene().buildIndex;
        vidaJogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vHealthController>();
        menuTituloText = menuTitulo.GetComponent<Text>();
        menuTituloText.text = "";
        menuTextoExtraText = menuTextoExtra.GetComponent<Text>();
        menuTextoExtraText.text = "";
        StaticClass.estadoDeJogo = 0;
        perdeu = false;
        venceu = false;
    }

    void Update()
    {
        if(vidaJogador.currentHealth <= 0 && perdeu == false)
        {
            StartCoroutine(Perder());
        }

        if (StaticClass.estadoDeJogo == 1)
        {
            menuContinuar.SetActive(true);
            menuTituloText.text = "VITÓRIA!";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if(venceu == false)
            {
                venceu = true;
                menuTitulo.GetComponent<Animator>().Play("CanvasBotoesTitulo");
                menuTextoExtraText.text = "Todos os inimigos foram derrotados!";
            }
        }
        else
        {
            menuContinuar.SetActive(false);
        }

        if (StaticClass.estadoDeJogo == -1)
        {
            menuPerdeu.SetActive(true);
            menuTituloText.text = "DERROTA!";
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

    IEnumerator Perder()
    {
        Debug.Log("Morreu");
        perdeu = true;

        yield return new WaitForSeconds(3f);

        StaticClass.estadoDeJogo = -1;
        menuTitulo.GetComponent<Animator>().Play("CanvasBotoesTitulo");
        MostrarDica();
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

    public void MostrarDica()
    {
        int dica = Random.Range(0, 2);
        if (dica == 0)
        {
            menuTextoExtraText.text = "Dica: Durante um rolamento (tecla Q), você não leva dano de ataques corpo a corpo, mas inimigos ainda perderão tempo tentando te atacar.";
        }
        else if (dica == 1)
        {
            menuTextoExtraText.text = "Dica: Ao se defender, você leva menos dano, mas ainda pode ser derrotado.";
        }
    }
}
