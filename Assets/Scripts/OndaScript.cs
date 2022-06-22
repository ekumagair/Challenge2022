using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaScript : MonoBehaviour
{
    public float delayInicial = 1f;
    public float delayRepetir = 1f;
    public int esperarOndas = 0;
    public bool esperarInimigosMortos = false;
    public GameObject inimigo;
    public Transform[] local;
    public int quantos = 1;
    public int repetir = 1;

    // Ordem de espera:
    // EsperarOndas > EsperarInimigosMortos > DelayInicial > DelayRepetir

    private void Start()
    {
        StaticClass.ondasPassadas = 0;
        StaticClass.inimigosMortos = 0;
        StaticClass.inimigosVivos = 0;
        StartCoroutine(EsperarInimigos());
    }

    IEnumerator EsperarInimigos()
    {
        if (esperarOndas > 0)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            while (StaticClass.ondasPassadas < esperarOndas)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (esperarInimigosMortos == true)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            while (StaticClass.inimigosVivos > 0)
            {
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(delayInicial);

        StartCoroutine(Criar());
    }

    IEnumerator Criar()
    {

        int escolha = Random.Range(0, local.Length);

        for (int i = 0; i < quantos; i++)
        {
            Instantiate(inimigo, local[escolha].position, local[escolha].rotation);
        }

        repetir--;

        if(repetir > 0)
        {
            yield return new WaitForSeconds(delayRepetir);
            StartCoroutine(Criar());
        }
        else
        {
            StaticClass.ondasPassadas++;
            Destroy(gameObject);
        }
    }
}
