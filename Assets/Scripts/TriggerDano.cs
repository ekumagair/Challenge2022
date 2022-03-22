using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDano : MonoBehaviour
{
    public int dano = 10;
    public float duracao = 0.2f;

    void Start()
    {
        Destroy(gameObject, duracao);
    }
}
