using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerSeFase : MonoBehaviour
{
    // Um objeto que tem este script é destruído se a fase atual não for igual a fase especificada. Isso não é visto pelo jogador.

    public enum Operador
    {
        Igual,
        MaiorQue,
        MenorQue
    }

    public Operador operador = 0;

    public int qualFase = 1;

    void Start()
    {
        switch (operador)
        {
            case Operador.Igual:
                if (StaticClass.faseAtual != qualFase)
                {
                    Destroy(gameObject);
                }
                break;

            case Operador.MaiorQue:
                if (StaticClass.faseAtual <= qualFase)
                {
                    Destroy(gameObject);
                }
                break;

            case Operador.MenorQue:
                if (StaticClass.faseAtual >= qualFase)
                {
                    Destroy(gameObject);
                }
                break;

            default:
                break;
        }
    }
}
