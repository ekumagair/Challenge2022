using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticClass
{
    public static int faseAtual = 1;
    public static int faseDesbloqueada = 1;
    public static int inimigosMortos = 0;
    public static int inimigosVivos = 0;
    public static int ondasPassadas = 0;
    public static int totalDeInimigos = 0;
    public static int segundosVivo = 0;
    public static bool viuAgradecimento = false;

    // Modo de jogo
    public static int modoDeJogo = 0;
    // 0 = Normal (Fase pr�-definida)
    // 1 = Fase infinita
    // 2 = Tempo limitado (Fase pr�-definida)
    // 3 = Demonstra��o

    // Usado na fase infinita
    public static int pontosDeDificuldade = 0;
    public static int inimigosMortosRecorde = 0;

    // Usado no modo de tempo limitado
    public static int tempoLimitadoMinutos = 0;
    public static int tempoLimitadoSegundos = 0;

    // Usado nos menus
    public static bool clicouEmBotao = false;

    // Estado de jogo
    public static int estadoDeJogo = 0;
    // 0 = Jogando
    // 1 = Venceu
    // 2 = Pausado
    // -1 = Perdeu

    // Op��es
    public static float sensibilidadeMouse = 8f;
    public static float volumeGlobal = 1.0f;
    public static float shakeMult = 1.0f;
    public static float volumeHitSound = 0.2f;

    // Modo debug: Permite algumas trapa�as e outras fun��es se ativado. Deve ser desativado na build definitiva do jogo.
    public static bool debug = true;

    // Tipo de invent�rio
    public static int tipoDeInventario = 1;
    // 0 = Usar as teclas 1, 2 e 3 para escolher os itens.
    // 1 = Usar uma tecla para alternar entre 2 armas e usar outra tecla para usar a po��o.

    // M�sica
    public static float musicaMenuTempo = 0;

    // Salvar dados
    public static void Salvar()
    {
        if (faseDesbloqueada < 1)
        {
            faseDesbloqueada = 1;
        }

        SaveSystem.SaveData();
    }

    // Carregar dados
    public static void Carregar()
    {
        UserData dados = SaveSystem.LoadData();

        if(dados != null)
        {
            faseDesbloqueada = dados.faseDesbloqueada;
            inimigosMortosRecorde = dados.inimigosMortosRecorde;
            sensibilidadeMouse = dados.sensibilidadeMouse;
            volumeGlobal = dados.volumeGlobal;
            AudioListener.volume = volumeGlobal;
            volumeHitSound = dados.volumeHitSound;
            shakeMult = dados.shakeMult;
        }
    }
}