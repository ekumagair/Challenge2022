using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectarPerda : MonoBehaviour
{
    // Objeto que detecta os diferentes estados de jogo e executa os efeitos necessários.

    Invector.vHealthController vidaJogador;
    bool perdeu = false;
    bool venceu = false;
    int cenaAtual;

    public GameObject menuContinuar;
    public GameObject menuPerdeu;
    public GameObject menuPausado;
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

        // Venceu
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

        // Perdeu
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

        // Pausou
        if (StaticClass.estadoDeJogo == 2)
        {
            menuPausado.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            menuPausado.SetActive(false);
        }

        // Trapaças de teste
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

        // Input para pausar ou continuar jogo
        if (Input.GetKeyDown(KeyCode.Escape) && StaticClass.estadoDeJogo != 1 && StaticClass.estadoDeJogo != -1)
        {
            //Application.Quit(); Teste
            //VoltarParaTitulo(); Teste

            if(Time.timeScale > 0)
            {
                Pausar(true);
            }
            else
            {
                Pausar(false);
            }
        }
    }

    IEnumerator Perder()
    {
        if (StaticClass.debug)
        {
            Debug.Log("Morreu");
        }

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

    // Quando o jogador vence a fase e vai para a próxima.
    public void Continuar()
    {
        StaticClass.faseAtual++;
        ReiniciarCena();
    }

    public void Pausar(bool pausar)
    {
        // Se o bool for verdadeiro, pausa. Senão, continua o jogo.
        if(pausar == true)
        {
            StaticClass.estadoDeJogo = 2;
            Time.timeScale = 0;
        }
        else
        {
            StaticClass.estadoDeJogo = 0;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

    // Mostra uma dica aleatória quando o jogador perde. As dicas são para todas as fases.
    public void MostrarDica()
    {
        int dica = Random.Range(0, 5);
        if (dica == 0)
        {
            menuTextoExtraText.text = "Dica: Durante um rolamento (tecla [Q]), você não leva dano de ataques corpo a corpo, mas inimigos ainda perderão tempo tentando te atacar.";
        }
        else if (dica == 1)
        {
            menuTextoExtraText.text = "Dica: Ao se defender (segurando o [botão direito do mouse]), você leva menos dano, mas ainda pode ser derrotado.";
        }
        else if (dica == 2)
        {
            menuTextoExtraText.text = "Dica: Você pode pular (com a [Barra De Espaço]) para desviar de projéteis e passar por cima de inimigos.";
        }
        else if (dica == 3)
        {
            menuTextoExtraText.text = "Dica: Ao usar um ataque forte (tecla [E]), você causa mais dano, mas gasta mais tempo que um ataque normal (feito com o [botão esquerdo do mouse]).";
        }
        else if (dica == 4)
        {
            menuTextoExtraText.text = "Dica: Depois de derrotar inimigos o suficiente, você pode executar um ataque especial apertando a tecla [F], causando dano nos inimigos perto de você.";
        }
    }
}
