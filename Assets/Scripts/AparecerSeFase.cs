using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerSeFase : MonoBehaviour
{
    // Um objeto que tem este script é destruído se a fase atual não for igual a fase especificada. Isso não é visto pelo jogador.

    public int qualFase = 1;

    void Start()
    {
        if(StaticClass.faseAtual != qualFase)
        {
            Destroy(gameObject);
        }
    }
}
