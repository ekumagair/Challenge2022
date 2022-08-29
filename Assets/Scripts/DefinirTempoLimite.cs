using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinirTempoLimite : MonoBehaviour
{
    public int minutos = 1;
    public int segundos = 0;

    void Start()
    {
        StaticClass.tempoLimitadoMinutos = minutos;
        StaticClass.tempoLimitadoSegundos = segundos;
    }
}
