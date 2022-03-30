using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacionar : MonoBehaviour
{
    public float velocidadeY = 100;

    void Update()
    {
        transform.Rotate(0, velocidadeY * Time.deltaTime, 0);
    }
}
