using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerSeFase : MonoBehaviour
{
    // Um objeto que tem este script � destru�do se a fase atual n�o for igual a fase especificada. Isso n�o � visto pelo jogador.

    public int qualFase = 1;

    void Start()
    {
        if(StaticClass.faseAtual != qualFase)
        {
            Destroy(gameObject);
        }
    }
}
