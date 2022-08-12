using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndasInfinitas : MonoBehaviour
{
    // Script da fase infinita.
    // Esta fase usa a variável "pontosDeDificuldade". Quanto maior for esse valor, mais inimigos o jogo pode criar.

    public GameObject[] inimigos;
    public int[] inimigosCusto;
    public int[] inimigosChance;
    public Transform[] locais;

    int listaTamanho;

    void Start()
    {
        StaticClass.pontosDeDificuldade = 10;
        listaTamanho = inimigos.Length - 1;
        StartCoroutine(Comecar());
    }

    IEnumerator Comecar()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("LISTA: " + listaTamanho);
        StartCoroutine(CriarOnda(3f));
    }
    
    IEnumerator CriarOnda(float delayInicial)
    {
        // Inimigos criados nesta onda.
        int inimigosCriados = 0;
        Debug.Log("Prestes a criar onda...");

        yield return new WaitForSeconds(delayInicial);

        Debug.Log("Criando onda");

        // Primeiro "for": Verificar todos os inimigos da lista.
        for (int i = listaTamanho; i >= 0; i--)
        {
            Debug.Log("Analisando condições do inimigo " + i);

            // Segundo "for": Criar um mesmo tipo de inimigo uma certa quantidade de vezes.
            for (int x = 0; x < Mathf.RoundToInt(StaticClass.pontosDeDificuldade / inimigosChance[i]); x++)
            {
                // Chance de criar ou não um inimigo. Alguns inimigos são mais frequentes que outros.
                if (Random.Range(0, inimigosChance[i]) == 0)
                {
                    // Inimigos custam pontos de dificuldade para serem criados.
                    if (StaticClass.pontosDeDificuldade >= inimigosCusto[i])
                    {
                        int escolha = Random.Range(0, locais.Length);

                        Instantiate(inimigos[i], locais[escolha].transform.position, locais[escolha].transform.rotation);
                        StaticClass.pontosDeDificuldade -= inimigosCusto[i];
                        inimigosCriados++;
                        StaticClass.totalDeInimigos++;

                        Debug.Log("Criou inimigo " + i);
                    }
                    else
                    {
                        // Não criou o inimigo porque não tem pontos o suficiente.
                        Debug.Log("Sem pontos para o inimigo " + i + " (" + StaticClass.pontosDeDificuldade + "/" + inimigosCusto[i] + ")");
                    }
                }
                else
                {
                    // Não criou o inimigo por causa da chance aleatória.
                    Debug.Log("Não criou inimigo " + i);
                }
            }
        }

        // Sempre aumentar os pontos de dificuldade depois de criar uma onda.
        StaticClass.pontosDeDificuldade++;

        Debug.Log("Criou " + inimigosCriados + " inimigos! " + StaticClass.pontosDeDificuldade + " pontos restantes.");

        // Se não criou nenhum inimigo, adiciona mais pontos para poder criar na próxima.
        if (inimigosCriados == 0)
        {
            StaticClass.pontosDeDificuldade += 10;
        }

        // Caso tenha progredido na fase, adiciona mais pontos para criar ainda mais inimigos.
        if(StaticClass.totalDeInimigos > 30)
        {
            StaticClass.pontosDeDificuldade += 10;
        }
        if (StaticClass.totalDeInimigos > 60)
        {
            StaticClass.pontosDeDificuldade += 10;
        }

        // Esperar todos os inimigos morrerem antes de criar a próxima onda.
        while (StaticClass.inimigosVivos > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(CriarOnda(3f));
    }
}
