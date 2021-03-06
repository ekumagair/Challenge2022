using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VencerFase : MonoBehaviour
{
    public int esperarOndas = 0;
    public float delay = 0;
    GameObject jogador;
    Invector.vCharacterController.vThirdPersonInput jogadorInput;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        jogadorInput = jogador.GetComponent<Invector.vCharacterController.vThirdPersonInput>();
        StaticClass.estadoDeJogo = 0;
        StartCoroutine(Esperar());
    }

    IEnumerator Esperar()
    {
        // Esperar uma quantidade de ondas ser criada.
        while (StaticClass.ondasPassadas < esperarOndas)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(delay);

        // Esperar todos os inimigos morrerem.
        while (StaticClass.inimigosVivos > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Muda o estado de jogo para vit?ria.
        StaticClass.estadoDeJogo = 1;
        Cursor.lockState = CursorLockMode.None;

        DesativarInputs();

        Debug.Log("Venceu a fase");
    }

    public void DesativarInputs()
    {
        if (jogadorInput != null)
        {
            Destroy(jogadorInput);
        }
    }
}
