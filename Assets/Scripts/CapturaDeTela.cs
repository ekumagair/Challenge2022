using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturaDeTela : MonoBehaviour
{
    void Update()
    {
        // Se o modo debug está ativado, você pode apertar a tecla O para tirar uma captura de tela. Ela aparece na pasta do jogo e tem um nome aleatório baseado na versão do jogo.
        if((Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.F5)) && StaticClass.debug == true)
        {
            ScreenCapture.CaptureScreenshot("Guerreiros Mundiais " + Application.version + " " + Random.Range(0, 20000).ToString() + ".png");
        }
    }
}
