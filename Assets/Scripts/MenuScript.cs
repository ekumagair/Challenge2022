using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Slider sliderMouse;
    public bool escSair;

    private void Start()
    {
        if(sliderMouse != null && StaticClass.sensibilidadeMouse != 0f)
        {
            sliderMouse.value = StaticClass.sensibilidadeMouse;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        SceneManager.LoadScene("Gameplay");
    }

    public void IrParaTitulo()
    {
        SceneManager.LoadScene("Titulo");
    }

    public void IrParaOpcoes()
    {
        SceneManager.LoadScene("Opcoes");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    public void MudarSensitividadeDoMouse()
    {
        StaticClass.sensibilidadeMouse = sliderMouse.value;
    }
}
