using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaScript : MonoBehaviour
{
    public float delay;
    public GameObject inimigo;
    public Transform[] local;
    public int quantos = 1;
    public int repetir = 1;

    private void Awake()
    {
        StartCoroutine(Criar());
    }

    IEnumerator Criar()
    {
        yield return new WaitForSeconds(delay);

        int escolha = Random.Range(0, local.Length);

        for (int i = 0; i < quantos; i++)
        {
            Instantiate(inimigo, local[escolha].position, local[escolha].rotation);
        }

        repetir--;

        if(repetir > 0)
        {
            StartCoroutine(Criar());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
