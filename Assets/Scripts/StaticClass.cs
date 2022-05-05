using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{
    public static int faseAtual = 1;
    public static int inimigosMortos = 0;
    public static int inimigosVivos = 0;
    public static int ondasPassadas = 0;

    public static int estadoDeJogo = 0;
    // 0 = Jogando
    // 1 = Venceu
    // -1 = Perdeu

    public static float sensibilidadeMouse = 8f;
}