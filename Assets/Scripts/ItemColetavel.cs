using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    // Item que o jogador pode pegar. Mais informações no script do próprio jogador.

    public int desbloquearArma = -1;
    public int recuperarVida = 0;
    public GameObject criarAoSerDestruido;
    public AudioClip clipAoSerDestruido;

    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        // Só deixa o jogador coletar se o item estiver parado. Esta colisão é apenas a colisão com o jogador. A colisão com o chão e paredes está em outro objeto que faz parte do prefab do item.
        if(rb.velocity.y != 0)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
}
