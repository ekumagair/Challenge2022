using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public bool jogador = false;
    public GameObject particulaDano;

    private void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

    public void CriarParticulaDano()
    {
        Instantiate(particulaDano, transform.position + transform.up, transform.rotation);
    }

    public void MatouInimigo()
    {
        if (jogador == false)
        {
            StaticClass.inimigosMortos++;
            Jogador.inimigosMortosHabilidade++;
            Debug.Log("INIMIGOS MORTOS: " + StaticClass.inimigosMortos);
        }
    }
}
