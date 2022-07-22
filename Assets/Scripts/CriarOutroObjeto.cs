using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriarOutroObjeto : MonoBehaviour
{
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
