using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    // O ninja corre rapidamente pelo mapa, usando pontos invisíveis como referência.

    public bool destruirAoMorrer = false;
    bool morto = false;

    bool atacando = false;
    public GameObject projetil;

    public bool podeFicarInvisivel = true;
    bool invisivel = false;
    public Material[] meshDefaultMat;
    public SkinnedMeshRenderer[] mesh;
    public Material materialInvisivel;

    public GameObject destino;
    public GameObject[] destinos;
    int destinosAntesDeAtacar = 3;
    NavMeshAgent agente;
    Animator animator;

    // destinosAntesDeAtacar = Quantos destinos o ninja escolhe antes de obrigatoriamente escolher o jogador. Se este limite não existir, pode demorar muito tempo para o jogador ser atacado.

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        morto = false;
        atacando = false;
        invisivel = false;

        destinos = GameObject.FindGameObjectsWithTag("Destino");
        destinos[0] = GameObject.FindGameObjectWithTag("Player");
        destinosAntesDeAtacar = 3;
        EscolherDestino(0);
    }

    void Update()
    {
        if (morto == false)
        {
            // Enqaunto não ataca, se move para seu destino.
            if (atacando == false)
            {
                agente.destination = destino.transform.position;
                animator.SetBool("Movendo", true);
            }
            else
            {
                // Ficar parado.
                agente.destination = transform.position;
                animator.SetBool("Movendo", false);
            }

            if (destino.tag == "Destino")
            {
                // Se o destino for apenas um ponto invisível no mapa, se aproxima dele troca de destino.
                if (agente.remainingDistance < 1)
                {
                    EscolherDestino(0);
                }
            }
            else if (destino.tag == "Player")
            {
                // Se o destino for o jogador, se aproxima dele e lança o projétil.
                if (agente.remainingDistance < 8 && atacando == false && Vector3.Distance(gameObject.transform.position, destino.transform.position) < 9)
                {
                    StartCoroutine(Atacar());
                }
            }

            /*
            transform.LookAt(destino.transform.position); Protótipo

            Vector3 resetar = transform.rotation.eulerAngles;
            resetar.x = 0;
            resetar.z = 0;

            transform.rotation = Quaternion.Euler(resetar);
            */

            // Olhar para o alvo gradualmente.
            Vector3 dir = destino.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        animator.SetBool("Morto", morto);
    }

    public void EscolherDestino(int min)
    {
        // Destino [0] = Atacar jogador. Ou seja, o primeiro espaço dessa lista é reservado para o jogador.
        // Destino [1]+ = Outros pontos invisíveis no mapa.

        destino = destinos[Random.Range(min, destinos.Length)];

        destinosAntesDeAtacar--;

        if (destinosAntesDeAtacar <= 0 && destino != destinos[0])
        {
            destinosAntesDeAtacar = Random.Range(2, 4);
            destino = destinos[0];
        }

        if(podeFicarInvisivel == true)
        {
            if (invisivel == false)
            {
                // 50% de chance de ficar transparente ao chegar no destino. O bool "podeFicarInvisivel" controla se essa chance existe ou não.
                if (Random.Range(0, 2) == 0)
                {
                    FicarTransparente();
                }
            }
            else if (meshDefaultMat[0] != null)
            {
                FicarOpaco();
            }
        }
    }

    IEnumerator Atacar()
    {
        atacando = true;
        animator.Play("Fire-Straight");

        yield return new WaitForSeconds(0.15f);

        var tiro = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        tiro.GetComponent<Projetil>().ignorar = "Enemy";
        tiro.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(2f);

        // Ao terminar o ataque, escolhe um destino que não é o jogador.
        EscolherDestino(1);
        atacando = false;
    }

    public void Morrer()
    {
        morto = true;

        if (destruirAoMorrer == true)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(GetComponent<Collider>());
            animator.Play("Death");
        }

        // Ficar parado.
        agente.destination = transform.position;
    }

    void FicarTransparente()
    {
        int qualMesh = 0;

        foreach (SkinnedMeshRenderer mr in mesh)
        {
            meshDefaultMat[qualMesh] = mr.material;
            mr.material = materialInvisivel;
            qualMesh++;
        }

        invisivel = true;
    }

    void FicarOpaco()
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].material = meshDefaultMat[i];
        }

        invisivel = false;
    }

    public void LevarDano()
    {
        animator.Play("BlockBreak");
    }
}
