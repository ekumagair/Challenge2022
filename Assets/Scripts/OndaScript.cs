using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaScript : MonoBehaviour
{
    // Objeto que cria inimigos. Usado em fases de dura??o finita.

    public float delayInicial = 1f;
    public float delayRepetir = 1f;
    public int esperarOndas = 0;
    public bool esperarInimigosMortos = false;
    public GameObject inimigo;
    public Transform[] local;
    public int quantos = 1;
    public int repetir = 1;

    // quantos = Quantidade de inimigos criadas em 1 local.
    // repetir = Quantas vezes a quantidade "quantos" de inimigos deve ser criada.

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
        // Esperar uma quantidade de ondas ter sido totalmente criada antes de criar os inimigos desta. N?o espera os inimigos morrerem.
        if (esperarOndas > 0)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            // Enquanto a condi??o for falsa, espera mais tempo.
            while (StaticClass.ondasPassadas < esperarOndas)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Espera at? que n?o haja nenhum inimigo vivo para criar esta onda.
        if (esperarInimigosMortos == true)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            // Enquanto a condi??o for falsa, espera mais tempo.
            while (StaticClass.inimigosVivos > 0)
            {
                yield return new WaitForSeconds(1f);
            }
        }

        // Delay inicial em segundos.
        yield return new WaitForSeconds(delayInicial);

        StartCoroutine(Criar());
    }

    IEnumerator Criar()
    {
        // Escolher um dos locais aleatoriamente.
        int escolha = Random.Range(0, local.Length);

        for (int i = 0; i < quantos; i++)
        {
            // Criar o objeto do inimigo.
            Instantiate(inimigo, local[escolha].position, local[escolha].rotation);
        }

        repetir--;

        // Se vai repetir, espera "delayRepetir" segundos e chama a corrotina de novo. Sen?o, destroi esta onda e ela ? considerada totalmente criada.
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
