using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public GameObject splashEspada;

    public void CriarSplashEspada()
    {
        Instantiate(splashEspada, transform.position + transform.forward, transform.rotation);
    }
}
