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
    Invector.vHealthController vida;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        vida = GetComponent<Invector.vHealthController>();
        alvo = GameObject.FindGameObjectWithTag("Player");
        agente.speed = 5f;
        morto = false;
        atacando = false;
        levandoDano = false;
        vida.isImmortal = false;
    }

    void Update()
    {
        tempoAtaque += Time.deltaTime;

        if (morto == false && atacando == false && alvo != null && levandoDano == false)
        {
            if (Mathf.Abs(Vector3.Distance(alvo.transform.position, transform.position)) > distanciaLimite && !levandoDano)
            {
                agente.destination = alvo.transform.position;
                animator.SetBool("Movendo", true);
            }
            else
            {
                agente.destination = transform.position;
                animator.SetBool("Movendo", false);

                if (!levandoDano && alvo != null && atacando == false)
                {
                    if (lancas > 0)
                    {
                        if (tempoAtaque > 5)
                        {
                            StartCoroutine(AtirarLanca());
                        }
                    }
                }
            }
        }
        else if(morto == false)
        {
            if (levandoDano == false || morto == true)
            {
                agente.destination = transform.position;
            }
            else
            {
                agente.destination = transform.forward * -1f;
            }

            animator.SetBool("Movendo", false);
        }

        if(morto == false)
        {
            Vector3 dir = alvo.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    IEnumerator AtirarLanca()
    {
        atacando = true;
        animator.Play("");
        lancas--;
        tempoAtaque = 0;

        yield return new WaitForSeconds(0.4f);

        var lanca = Instantiate(projetil, transform.position + (transform.up * 1.25f) + transform.forward, transform.rotation);
        lanca.GetComponent<Projetil>().ignorar = "Enemy";
        lanca.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(0.5f);

        if (lancas > 0)
        {
            distanciaLimite *= 0.45f;
        }
        else
        {
            distanciaLimite = 1.5f;
            agente.speed = 3f;
            StartCoroutine(RecuperarLanca());
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
        animator.Play("BlockBreak");
        StartCoroutine(Invulneravel());
    }

    IEnumerator Invulneravel()
    {
        vida.isImmortal = true;
        levandoDano = true;

        yield return new WaitForSeconds(2f);

        vida.isImmortal = false;
        levandoDano = false;
    }

    IEnumerator RecuperarLanca()
    {
        yield return new WaitForSeconds(5f);

        agente.speed = 5f;
        lancas = 1;
    }
}
