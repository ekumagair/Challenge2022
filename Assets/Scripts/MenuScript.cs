using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider sliderMouse;
    public Slider sliderVolume;
    public Slider sliderVolumeHit;
    public AudioSource sliderVolumeHitAS;
    public Slider sliderShakeMult;
    public Text mouseValor;
    public Text volumeValor;
    public Text volumeHitValor;
    public Text shakeMultValor;

    public Text textBuild;
    public Text textCarregando;
    public bool escSair;

    void Start()
    {
        if(sliderMouse != null && StaticClass.sensibilidadeMouse != 0f)
        {
            sliderMouse.value = StaticClass.sensibilidadeMouse;
        }

        if (sliderVolume != null)
        {
            sliderVolume.value = StaticClass.volumeGlobal;
        }

        if (sliderVolumeHit != null)
        {
            sliderVolumeHit.value = StaticClass.volumeHitSound;
        }

        if (sliderShakeMult != null)
        {
            sliderShakeMult.value = StaticClass.shakeMult;
        }

        if (textBuild != null)
        {
            if (StaticClass.debug)
            {
                textBuild.text = "Build " + Application.version.ToString();
            }
            else
            {
                textBuild.text = "Vers�o " + Application.version.ToString();
            }
        }

        if(textCarregando != null)
        {
            textCarregando.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        StaticClass.totalDeInimigos = 0;
        StaticClass.clicouEmBotao = false;
        StaticClass.segundosVivo = 0;

        StaticClass.estadoDeJogo = 0;
        StaticClass.ondasPassadas = 0;
        StaticClass.inimigosMortos = 0;
        StaticClass.inimigosVivos = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se o esc faz sair do jogo ou apenas voltar para o menu de t�tulo.
            if (escSair)
            {
                SairDoJogo();
            }
            else
            {
                IrParaTitulo();
            }
        }

        // Texto que representa os valores das op��es.
        if (mouseValor != null)
        {
            mouseValor.text = "(" + Mathf.RoundToInt(StaticClass.sensibilidadeMouse).ToString() + ")";
        }
        if (volumeValor != null)
        {
            volumeValor.text = "(" + Mathf.RoundToInt(StaticClass.volumeGlobal * 100f).ToString() + "%)";
        }
        if (volumeHitValor != null)
        {
            volumeHitValor.text = "(" + Mathf.RoundToInt(StaticClass.volumeHitSound * 100f).ToString() + "%)";
        }
        if (shakeMultValor != null)
        {
            shakeMultValor.text = "(" + Mathf.RoundToInt(StaticClass.shakeMult).ToString() + "x)";
        }

        // Trapa�as de teste
        if (Input.GetKeyDown(KeyCode.Comma) && StaticClass.debug)
        {
            StaticClass.faseDesbloqueada--;
        }
        if (Input.GetKeyDown(KeyCode.Period) && StaticClass.debug)
        {
            StaticClass.faseDesbloqueada++;
        }
    }

    public void IrParaJogo()
    {
        //SceneManager.LoadScene("Gameplay"); Prot�tipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaFase(int fase)
    {
        StaticClass.faseAtual = fase;
        //SceneManager.LoadScene("Gameplay"); Prot�tipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaSelecionarFase()
    {
        //SceneManager.LoadScene("SelecionarFases"); Prot�tipo
        StartCoroutine(IrPara("SelecionarFases"));
    }

    public void IrParaTitulo()
    {
        //SceneManager.LoadScene("Titulo"); Prot�tipo
        StartCoroutine(IrPara("Titulo"));
    }

    public void IrParaOpcoes()
    {
        //SceneManager.LoadScene("Opcoes"); Prot�tipo
        StartCoroutine(IrPara("Opcoes"));
    }

    public void IrParaControles()
    {
        //SceneManager.LoadScene("Controles"); Prot�tipo
        StartCoroutine(IrPara("Controles"));
    }

    public void IrParaCreditos()
    {
        //SceneManager.LoadScene("Creditos"); Prot�tipo
        StartCoroutine(IrPara("Creditos"));
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    IEnumerator IrPara(string cena)
    {
        // Se tem o texto de carregamento no layout, mostra ele.
        if (textCarregando != null)
        {
            textCarregando.enabled = true;
        }

        // Se n�o houver delay, o som dos bot�es n�o toca.
        if (StaticClass.clicouEmBotao == false)
        {
            StaticClass.clicouEmBotao = true;
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(cena);
        }
    }

    // Op��es
    public void MudarSensitividadeDoMouse()
    {
        StaticClass.sensibilidadeMouse = sliderMouse.value;
    }

    public void MudarVolumeGlobal()
    {
        StaticClass.volumeGlobal = sliderVolume.value;
        AudioListener.volume = StaticClass.volumeGlobal;
    }

    public void MudarVolumeHit()
    {
        StaticClass.volumeHitSound = sliderVolumeHit.value;
        sliderVolumeHitAS.volume = StaticClass.volumeHitSound;
    }

    public void MudarShakeMult()
    {
        StaticClass.shakeMult = sliderShakeMult.value;
    }
}
