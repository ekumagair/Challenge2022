using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{
    public static int faseAtual = 1;
    public static int inimigosMortos = 0;
    public static int inimigosVivos = 0;
    public static int ondasPassadas = 0;

    public static bool clicouEmBotao = false;
    // Usado nos menus.

    public static int estadoDeJogo = 0;
    // 0 = Jogando
    // 1 = Venceu
    // 2 = Pausado
    // -1 = Perdeu

    // Opções
    public static float sensibilidadeMouse = 8f;
    public static float volumeGlobal = 1.0f;
    public static float shakeMult = 1.0f;

    // Modo debug: Permite algumas trapaças e outras funções se ativado. Deve ser desativado na build definitiva do jogo.
    public static bool debug = true;
}