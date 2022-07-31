using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriarOutroObjeto : MonoBehaviour
{
    // Um objeto que tem este componente cria outro objeto, em sua posição e rotação, depois de um delay em segundos. Apenas uma vez.

    public GameObject objeto;
    public float delay = 0f;
    
    void Start()
    {
        if (objeto != null)
        {
            StartCoroutine(Criar());
        }
    }

    IEnumerator Criar()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(objeto, transform.position, transform.rotation);
    }
}
