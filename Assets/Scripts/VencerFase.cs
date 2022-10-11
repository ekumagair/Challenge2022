using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VencerFase : MonoBehaviour
{
    public int esperarOndas = 0;
    GameObject jogador;

    public static bool matouTodosOsInimigos = false;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        matouTodosOsInimigos = false;
        StaticClass.estadoDeJogo = 0;
        StartCoroutine(Esperar());
    }

    IEnumerator Esperar()
    {
        yield return new WaitForSeconds(3.5f);

        if (StaticClass.debug)
        {
            Debug.Log("Fase " + StaticClass.faseAtual.ToString() + ": " + esperarOndas.ToString() + " ondas para vencer. " + StaticClass.ondasPassadas.ToString() + " ondas passadas.");
        }

        // Esperar uma quantidade de ondas ser criada.
        while (StaticClass.ondasPassadas < esperarOndas)
        {
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2.0f);

        // Esperar todos os inimigos morrerem.
        while (StaticClass.inimigosVivos > 0)
        {
            yield return new WaitForSeconds(0.01f);
        }

        // Muda o estado de jogo para vit�ria.
        matouTodosOsInimigos = true;
        Cursor.lockState = CursorLockMode.None;

        jogador.GetComponent<Jogador>().DesativarInputs();

        if (StaticClass.debug)
        {
            Debug.Log("Desativou inputs");
        }
    }
}
