using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    // Item que o jogador pode pegar. Mais informa��es no script do pr�prio jogador.

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
        // S� deixa o jogador coletar se o item estiver parado. Esta colis�o � apenas a colis�o com o jogador. A colis�o com o ch�o e paredes est� em outro objeto que faz parte do prefab do item.
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
