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
                textBuild.text = "Versão " + Application.version.ToString();
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        StaticClass.totalDeInimigos = 0;
        StaticClass.clicouEmBotao = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se o esc faz sair do jogo ou apenas voltar para o menu de título.
            if (escSair)
            {
                SairDoJogo();
            }
            else
            {
                IrParaTitulo();
            }
        }

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
    }

    public void IrParaJogo()
    {
        //SceneManager.LoadScene("Gameplay"); Protótipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaFase(int fase)
    {
        StaticClass.faseAtual = fase;
        //SceneManager.LoadScene("Gameplay"); Protótipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaSelecionarFase()
    {
        //SceneManager.LoadScene("SelecionarFases"); Protótipo
        StartCoroutine(IrPara("SelecionarFases"));
    }

    public void IrParaTitulo()
    {
        //SceneManager.LoadScene("Titulo"); Protótipo
        StartCoroutine(IrPara("Titulo"));
    }

    public void IrParaOpcoes()
    {
        //SceneManager.LoadScene("Opcoes"); Protótipo
        StartCoroutine(IrPara("Opcoes"));
    }

    public void IrParaControles()
    {
        //SceneManager.LoadScene("Controles"); Protótipo
        StartCoroutine(IrPara("Controles"));
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    IEnumerator IrPara(string cena)
    {
        // Se não houver delay, o som dos botões não toca.
        if (StaticClass.clicouEmBotao == false)
        {
            StaticClass.clicouEmBotao = true;
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(cena);
        }
    }

    // Opções
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
