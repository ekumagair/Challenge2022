using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider sliderMouse;
    public Slider sliderVolume;
    public Text mouseValor;
    public Text volumeValor;
    public Text textBuild;
    public bool escSair;

    private void Start()
    {
        if(sliderMouse != null && StaticClass.sensibilidadeMouse != 0f)
        {
            sliderMouse.value = StaticClass.sensibilidadeMouse;
        }

        if (sliderVolume != null)
        {
            sliderVolume.value = StaticClass.volumeGlobal;
        }

        if(textBuild != null)
        {
            if (StaticClass.debug)
            {
                textBuild.text = "Build " + Application.version.ToString();
            }
            else
            {
                textBuild.text = "";
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;

        StaticClass.clicouEmBotao = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se o esc faz sair do jogo ou apenas voltar para o menu de t?tulo.
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
    }

    public void IrParaJogo()
    {
        //SceneManager.LoadScene("Gameplay"); Prot?tipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaFase(int fase)
    {
        StaticClass.faseAtual = fase;
        //SceneManager.LoadScene("Gameplay"); Prot?tipo
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaSelecionarFase()
    {
        //SceneManager.LoadScene("SelecionarFases"); Prot?tipo
        StartCoroutine(IrPara("SelecionarFases"));
    }

    public void IrParaTitulo()
    {
        //SceneManager.LoadScene("Titulo"); Prot?tipo
        StartCoroutine(IrPara("Titulo"));
    }

    public void IrParaOpcoes()
    {
        //SceneManager.LoadScene("Opcoes"); Prot?tipo
        StartCoroutine(IrPara("Opcoes"));
    }

    public void IrParaControles()
    {
        //SceneManager.LoadScene("Controles"); Prot?tipo
        StartCoroutine(IrPara("Controles"));
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    IEnumerator IrPara(string cena)
    {
        // Se n?o houver delay, o som dos bot?es n?o toca.
        if (StaticClass.clicouEmBotao == false)
        {
            StaticClass.clicouEmBotao = true;
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(cena);
        }
    }

    // Op??es
    public void MudarSensitividadeDoMouse()
    {
        StaticClass.sensibilidadeMouse = sliderMouse.value;
    }

    public void MudarVolumeGlobal()
    {
        StaticClass.volumeGlobal = sliderVolume.value;
        AudioListener.volume = StaticClass.volumeGlobal;
    }
}
