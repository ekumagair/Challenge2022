using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AparecerSeFase : MonoBehaviour
{
    public int qualFase = 1;

    void Start()
    {
        if(StaticClass.faseAtual != qualFase)
        {
            Destroy(gameObject);
        }
    }
}
