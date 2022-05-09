using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldadoRomano : MonoBehaviour
{
    public int lancas = 1;
    public bool destruirAoMorrer = false;
    bool morto = false;

    float tempoAtaque = 0;
    bool atacando = false;
    public GameObject projetil;

    bool levandoDano = false;

    float distanciaLimite = 10;
    public GameObject alvo;
    NavMeshAgent agente;
    Animator animator;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        alvo = GameObject.FindGameObjectWithTag("Player");
        agente.speed = 5f;
        morto = false;
        atacando = false;
        levandoDano = false;
    }

    void Update()
    {
        tempoAtaque += Time.deltaTime;

        if (morto == false && atacando == false && alvo != null)
        {
            if (Mathf.Abs(Vector3.Distance(alvo.transform.position, transform.position)) > distanciaLimite && !levandoDano)
            {
                agente.destination = alvo.transform.position;
            }
            else
            {
                agente.destination = transform.position;

                if(!levandoDano && alvo != null && atacando == false && lancas > 0 && tempoAtaque > 5)
                {
                    StartCoroutine(AtirarLanca());
                }
            }
        }
        else
        {
            agente.destination = transform.position;
        }
    }

    IEnumerator AtirarLanca()
    {
        atacando = true;
        animator.Play("");
        lancas--;
        tempoAtaque = 0;

        yield return new WaitForSeconds(0.4f);

        var lanca = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        lanca.GetComponent<Projetil>().ignorar = "Enemy";
        lanca.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(0.5f);

        if (lancas > 0)
        {
            distanciaLimite *= 0.45f;
        }
        else
        {
            distanciaLimite = 3f;
            agente.speed = 2f;
        }

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
            animator.Play("");
        }

        agente.destination = transform.position;
    }

    public void LevarDano()
    {
        animator.Play("");
    }
}
