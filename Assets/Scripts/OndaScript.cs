using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndaScript : MonoBehaviour
{
    // Objeto que cria inimigos. Usado em fases de duração finita.

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
        StaticClass.totalDeInimigos = 1;
        StaticClass.inimigosMortos = 0;
        StaticClass.inimigosVivos = 0;
        StartCoroutine(EsperarInimigos());
        StartCoroutine(ContarInimigos());
    }

    IEnumerator EsperarInimigos()
    {
        // Esperar uma quantidade de ondas ter sido totalmente criada antes de criar os inimigos desta. Não espera os inimigos morrerem.
        if (esperarOndas > 0)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            // Enquanto a condição for falsa, espera mais tempo.
            while (StaticClass.ondasPassadas < esperarOndas)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Espera até que não haja nenhum inimigo vivo para criar esta onda.
        if (esperarInimigosMortos == true)
        {
            yield return new WaitForSeconds(delayInicial / 10);

            // Enquanto a condição for falsa, espera mais tempo.
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
            var ini = Instantiate(inimigo, local[escolha].position, local[escolha].rotation);

            if(ini.GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>() != null)
            {
                // Se o inimigo pode rolar, remove essa habilidade se ainda estiver no início da fase.
                if(StaticClass.ondasPassadas < 3)
                {
                    ini.GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>().chanceToRoll = 0.0f;
                }
            }

            if (StaticClass.modoDeJogo == 3)
            {
                if (ini.GetComponent<Invector.vMelee.vMeleeManager>() != null)
                {
                    ini.GetComponent<Invector.vMelee.vMeleeManager>().hitProperties.hitDamageTags[0] = "";
                }
            }
        }

        local[escolha].GetComponent<PontoDeSpawn>().DestaqueTochas();

        repetir--;

        // Se vai repetir, espera "delayRepetir" segundos e chama a corrotina de novo. Senão, destroi esta onda e ela é considerada totalmente criada.
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

    IEnumerator ContarInimigos()
    {
        yield return new WaitForSeconds(3f);
        StaticClass.totalDeInimigos += quantos * repetir;

        if (StaticClass.debug)
        {
            Debug.Log("TOTAL DE INIMIGOS: " + StaticClass.totalDeInimigos);
        }
    }
}
