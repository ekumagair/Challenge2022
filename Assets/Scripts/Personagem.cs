using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public bool jogador = false;
    public GameObject particulaDano;

    public void CriarParticulaDano()
    {
        Instantiate(particulaDano, transform.position + transform.up, transform.rotation);
    }
}
