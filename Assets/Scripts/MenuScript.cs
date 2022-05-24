using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider sliderMouse;
    public Slider sliderVolume;
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StaticClass.clicouEmBotao = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escSair)
            {
                SairDoJogo();
            }
            else
            {
                IrParaTitulo();
            }
        }
    }

    public void IrParaJogo()
    {
        //SceneManager.LoadScene("Gameplay");
        StartCoroutine(IrPara("Gameplay"));
    }

    public void IrParaTitulo()
    {
        //SceneManager.LoadScene("Titulo");
        StartCoroutine(IrPara("Titulo"));
    }

    public void IrParaOpcoes()
    {
        //SceneManager.LoadScene("Opcoes");
        StartCoroutine(IrPara("Opcoes"));
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    IEnumerator IrPara(string cena)
    {
        if (StaticClass.clicouEmBotao == false)
        {
            StaticClass.clicouEmBotao = true;
            yield return new WaitForSeconds(0.2f);
            SceneManager.LoadScene(cena);
        }
    }

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
