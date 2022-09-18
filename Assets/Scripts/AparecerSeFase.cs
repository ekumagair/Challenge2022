using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerSeFase : MonoBehaviour
{
    // Um objeto que tem este script � destru�do se a fase atual n�o for igual a fase especificada. Isso n�o � visto pelo jogador.

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
