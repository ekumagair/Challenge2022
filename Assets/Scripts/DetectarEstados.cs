using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectarEstados : MonoBehaviour
{
    // Objeto que detecta os diferentes estados de jogo e executa os efeitos necess�rios.

    Invector.vHealthController vidaJogador;
    Jogador scriptJogador;
    bool perdeu = false;
    bool venceu = false;
    int cenaAtual;

    public GameObject menuContinuar;
    public GameObject menuPerdeu;
    public GameObject menuPausado;
    public GameObject menuTitulo;
    public GameObject menuTextoExtra;
    public GameObject menuControles;
    public GameObject menuIrParaAgradecimento;

    public AudioClip clipBotao;
    public AudioClip clipPrimeiroInimigo;
    bool clipPrimeiroInimigoTocou = false;
    public AudioClip clipVenceu;
    public AudioClip clipVenceuInstante;
    public AudioClip clipPerdeu;
    public AudioClip clipPerdeuInstante;

    Text menuTituloText;
    Text menuTextoExtraText;

    void Start()
    {
        cenaAtual = SceneManager.GetActiveScene().buildIndex;
        vidaJogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vHealthController>();
        scriptJogador = GameObject.FindGameObjectWithTag("Player").GetComponent<Jogador>();
        menuTituloText = menuTitulo.GetComponent<Text>();
        menuTituloText.text = "";
        menuTextoExtraText = menuTextoExtra.GetComponent<Text>();
        menuTextoExtraText.text = "";
        menuPausado.SetActive(false);
        menuControles.SetActive(false);
        menuIrParaAgradecimento.SetActive(false);
        StaticClass.estadoDeJogo = 0;
        vidaJogador.isImmortal = false;
        clipPrimeiroInimigoTocou = false;
        perdeu = false;
        venceu = false;
    }

    void Update()
    {
        // Detectar vit�ria
        if(VencerFase.matouTodosOsInimigos == true && perdeu == false && venceu == false)
        {
            StartCoroutine(Vencer(3f));
        }

        // Detectar perda
        if (perdeu == false && venceu == false)
        {
            // Causar perda ao ficar sem vida
            if (vidaJogador.currentHealth <= 0)
            {
                StartCoroutine(Perder(3f));
            }

            // Causar perda ao ficar sem tempo
            if (StaticClass.modoDeJogo == 2 && StaticClass.tempoLimitadoMinutos == 0 && StaticClass.tempoLimitadoSegundos == 0)
            {
                StartCoroutine(Perder(0f));
            }
        }

        // Tocar som quando o primeiro inimigo aparecer
        if (StaticClass.inimigosVivos > 0 && clipPrimeiroInimigoTocou == false && clipPrimeiroInimigo != null)
        {
            clipPrimeiroInimigoTocou = true;
            scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipPrimeiroInimigo, false);
        }

        // Venceu
        if (StaticClass.estadoDeJogo == 1 && StaticClass.modoDeJogo != 3)
        {
            menuTituloText.text = "VIT�RIA!";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Executa uma s� vez.
            if(venceu == false)
            {
                venceu = true;
                menuTitulo.GetComponent<Animator>().Play("CanvasBotoesTitulo");
                menuTextoExtraText.text = "Todos os inimigos foram derrotados!";

                if(StaticClass.faseAtual == 5 && StaticClass.viuAgradecimento == false)
                {
                    menuContinuar.SetActive(false);
                    menuIrParaAgradecimento.SetActive(true);
                }
                else
                {
                    menuContinuar.SetActive(true);
                    menuIrParaAgradecimento.SetActive(false);
                }

                if(StaticClass.faseAtual == StaticClass.faseDesbloqueada)
                {
                    StaticClass.faseDesbloqueada++;
                }

                Salvar();
            }
        }
        else
        {
            menuContinuar.SetActive(false);
        }

        // Perdeu
        if (StaticClass.estadoDeJogo == -1 && StaticClass.modoDeJogo != 3)
        {
            menuPerdeu.SetActive(true);

            if (StaticClass.modoDeJogo == 0)
            {
                menuTituloText.text = "DERROTA!";
            }
            else if (StaticClass.modoDeJogo == 1)
            {
                menuTituloText.text = "FIM DE JOGO!";
            }
            else if (StaticClass.modoDeJogo == 2)
            {
                menuTituloText.text = "O TEMPO ACABOU!";
            }

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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Trapa�as de teste
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
        if (Input.GetKeyDown(KeyCode.Y) && StaticClass.debug)
        {
            ReiniciarCena();
        }

        // Input para pausar ou continuar jogo
        if (Input.GetKeyDown(KeyCode.Escape) && StaticClass.estadoDeJogo != 1 && StaticClass.estadoDeJogo != -1 && venceu == false && perdeu == false)
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

    IEnumerator Vencer(float delay)
    {
        VencerFase.matouTodosOsInimigos = false;
        vidaJogador.isImmortal = true;

        if (StaticClass.debug)
        {
            Debug.Log("Venceu");
        }

        if (clipVenceuInstante != null)
        {
            scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipVenceuInstante, false);
        }

        Time.timeScale = 0.01f;

        yield return new WaitForSeconds(0.008f);

        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(delay * 0.5f);

        if (clipVenceu != null)
        {
            scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipVenceu, true);
        }

        Time.timeScale = 1.0f;
        StaticClass.estadoDeJogo = 1;
    }

    IEnumerator Perder(float delay)
    {
        if (StaticClass.debug)
        {
            Debug.Log("Morreu");
        }

        if (clipPerdeuInstante != null)
        {
            scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipPerdeuInstante, false);
        }

        perdeu = true;
        scriptJogador.PararContagemDeTempo();
        scriptJogador.DestruirCamera();

        yield return new WaitForSeconds(delay);

        if (clipPerdeu != null)
        {
            scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipPerdeu, true);
        }

        StaticClass.estadoDeJogo = -1;
        menuTitulo.GetComponent<Animator>().Play("CanvasBotoesTitulo");
        MostrarDica();
        Salvar();
    }

    public void MudarEstadoDeJogo(int e)
    {
        StaticClass.estadoDeJogo = e;
    }

    // Quando o jogador vence a fase e vai para a pr�xima.
    public void Continuar()
    {
        StaticClass.faseAtual++;
        ReiniciarCena();
    }

    public void Pausar(bool pausar)
    {
        // Se o bool for verdadeiro, pausa. Sen�o, continua o jogo.
        if(pausar == true)
        {
            menuPausado.SetActive(true);
            StaticClass.estadoDeJogo = 2;
            Time.timeScale = 0;
        }
        else
        {
            menuPausado.SetActive(false);
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
        StaticClass.inimigosMortos = 0;
        StaticClass.inimigosVivos = 0;
        StaticClass.segundosVivo = 0;
    }

    public void ReiniciarCena()
    {
        Salvar();
        ReiniciarVariaveis();
        SceneManager.LoadScene(cenaAtual);
    }

    public void VoltarParaTitulo()
    {
        Salvar();
        SceneManager.LoadScene("Titulo");
    }

    public void IrParaAgradecimentos()
    {
        Salvar();
        ReiniciarVariaveis();
        StaticClass.musicaMenuTempo = 0;
        StaticClass.viuAgradecimento = true;
        SceneManager.LoadScene("Agradecimentos");
    }

    public void MostrarControles()
    {
        menuControles.SetActive(true);
        menuPausado.SetActive(false);
    }

    public void SairDosControles()
    {
        menuControles.SetActive(false);
        menuPausado.SetActive(true);
    }

    public void SomDeBotao()
    {
        scriptJogador.CriarObjetoDeSom(scriptJogador.audioSource2D, clipBotao, false);
    }

    // Mostra uma dica aleat�ria quando o jogador perde. As dicas s�o para todas as fases.
    public void MostrarDica()
    {
        if (StaticClass.modoDeJogo == 0)
        {
            int dica = Random.Range(0, 9);
            if (dica == 0)
            {
                menuTextoExtraText.text = "Dica: Durante um rolamento (tecla [Q]), voc� n�o leva dano de ataques corpo a corpo, mas inimigos ainda perder�o tempo tentando te atacar.";
            }
            else if (dica == 1)
            {
                menuTextoExtraText.text = "Dica: Ao se defender (segurando o [bot�o direito do mouse]), voc� leva menos dano, mas ainda pode ser derrotado.";
            }
            else if (dica == 2)
            {
                menuTextoExtraText.text = "Dica: Voc� pode pular (com a [Barra De Espa�o]) para desviar de proj�teis e passar por cima de inimigos.";
            }
            else if (dica == 3)
            {
                menuTextoExtraText.text = "Dica: Ao usar um ataque forte (tecla [E]), voc� causa mais dano, mas gasta mais tempo que um ataque normal (feito com o [bot�o esquerdo do mouse]).";
            }
            else if (dica == 4)
            {
                menuTextoExtraText.text = "Dica: Depois de derrotar inimigos o suficiente, voc� pode executar um ataque especial apertando a tecla [F], causando dano nos inimigos perto de voc�.";
            }
            else if (dica == 5)
            {
                menuTextoExtraText.text = "Dica: Ataques feitos com a espada podem refletir alguns proj�teis, fazendo com que causem dano em inimigos. Ataques feitos com o machado podem derrubar proj�teis, inutilizando-os.";
            }
            else if (dica == 6)
            {
                menuTextoExtraText.text = "Dica: Quando um inimigo aparece, as tochas do port�o em que ele estiver ficam mais fortes temporariamente.";
            }
            else if (dica == 7)
            {
                menuTextoExtraText.text = "Dica: Quando o primeiro inimigo de uma fase aparece, � poss�vel ouvir trombetas.";
            }
            else if (dica == 8)
            {
                menuTextoExtraText.text = "Dica: Se voc� conseguir atingir um inimigo enquanto ele quer te golpear, voc� far� ele cambalear.";
            }
        }
        else if (StaticClass.modoDeJogo == 1)
        {
            // Evitar valores imposs�veis.
            if (StaticClass.segundosVivo < 0)
            {
                StaticClass.segundosVivo = 0;
            }
            if (StaticClass.inimigosMortos < 0)
            {
                StaticClass.inimigosMortos = 0;
            }
            if (StaticClass.inimigosMortosRecorde < 0)
            {
                StaticClass.inimigosMortosRecorde = 0;
            }

            // Criar mensagem final da fase infinita.
            if (StaticClass.segundosVivo < 60)
            {
                menuTextoExtraText.text = "Por " + StaticClass.segundosVivo.ToString() + " segundos, ";
            }
            else if (StaticClass.segundosVivo < 120)
            {
                menuTextoExtraText.text = "Por 1 minuto, ";
            }
            else
            {
                menuTextoExtraText.text = "Por " + Mathf.RoundToInt(StaticClass.segundosVivo / 60).ToString() + " minutos, ";
            }

            if (StaticClass.inimigosMortos == 0)
            {
                menuTextoExtraText.text += "voc� n�o derrotou nenhum inimigo.";
            }
            else if (StaticClass.inimigosMortos == 1)
            {
                menuTextoExtraText.text += "voc� derrotou 1 inimigo.";
            }
            else
            {
                menuTextoExtraText.text += "voc� derrotou " + StaticClass.inimigosMortos.ToString() + " inimigos.";
            }

            if (StaticClass.inimigosMortosRecorde == 1)
            {
                menuTextoExtraText.text += " Seu recorde � de " + StaticClass.inimigosMortosRecorde.ToString() + " inimigo.";
            }
            else
            {
                menuTextoExtraText.text += " Seu recorde � de " + StaticClass.inimigosMortosRecorde.ToString() + " inimigos.";
            }
        }
        else if (StaticClass.modoDeJogo == 2)
        {
            //menuTextoExtraText.text = "Tente novamente.";
            int dica = Random.Range(0, 2);
            if (dica == 0)
            {
                menuTextoExtraText.text = "Dica: Refletir proj�teis com a espada � mais r�pido que perseguir os inimigos e acert�-los m�ltiplas vezes.";
            }
            else if (dica == 1)
            {
                menuTextoExtraText.text = "Dica: Se voc� tiver energia (barra verde) o suficiente, voc� pode segurar o [Shift] para correr.";
            }
        }
    }

    // Salvar
    public void Salvar()
    {
        if(StaticClass.faseDesbloqueada < 1)
        {
            StaticClass.faseDesbloqueada = 1;
        }

        PlayerPrefs.SetInt("fase_desbloqueada", StaticClass.faseDesbloqueada);
        PlayerPrefs.SetInt("inimigos_mortos_recorde", StaticClass.inimigosMortosRecorde);
    }
}
