using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Samurai : MonoBehaviour
{
    public bool destruirAoMorrer = false;
    bool morto = false;

    float tempoAteAtacar = 5;
    bool atacando = false;
    public GameObject projetil;

    bool levandoDano = false;

    float distanciaLimite = 5;
    public GameObject alvo;
    NavMeshAgent agente;
    Animator animator;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        alvo = GameObject.FindGameObjectWithTag("Player");
        morto = false;
        atacando = false;
        levandoDano = false;

        ResetarTempoDeAtaque();
        tempoAteAtacar += 2f;
        distanciaLimite = Random.Range(8f, 13f);
    }

    void Update()
    {
        if (morto == false && atacando == false && alvo != null)
        {
            if (Mathf.Abs(Vector3.Distance(alvo.transform.position, transform.position)) > distanciaLimite || levandoDano)
            {

                if (levandoDano == false)
                {
                    agente.destination = alvo.transform.position;
                }
                else
                {
                    agente.destination = transform.forward * -3f;
                }
            }
            else
            {
                agente.destination = transform.position;
            }

            if(agente.velocity.magnitude > 0)
            {
                animator.SetBool("Movendo", true);
            }
            else
            {
                animator.SetBool("Movendo", false);
            }

            /*
            transform.LookAt(alvo.transform.position);

            Vector3 resetar = transform.rotation.eulerAngles;
            resetar.x = 0;
            resetar.z = 0;

            transform.rotation = Quaternion.Euler(resetar);
            */

            Vector3 dir = alvo.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            tempoAteAtacar -= Time.deltaTime;

            if(tempoAteAtacar <= 0 && atacando == false)
            {
                StartCoroutine(Atacar());
            }
        }
        else
        {
            agente.destination = transform.position;
            animator.SetBool("Movendo", false);
        }

        animator.SetBool("Morto", morto);
        animator.SetBool("Fugindo", levandoDano);
    }

    IEnumerator Atacar()
    {
        atacando = true;
        ResetarTempoDeAtaque();
        animator.Play("Fire-Straight");

        yield return new WaitForSeconds(0.4f);

        var flecha = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        flecha.GetComponent<Projetil>().ignorar = "Enemy";
        flecha.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(0.5f);

        atacando = false;
    }

    public void ResetarTempoDeAtaque()
    {
        tempoAteAtacar = Random.Range(3f, 5f);
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

        agente.destination = transform.position;
    }

    public void LevarDano()
    {
        animator.Play("BlockBreak");
        StartCoroutine(LevarDanoCoroutine());
    }

    IEnumerator LevarDanoCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        levandoDano = true;
        yield return new WaitForSeconds(3f);
        levandoDano = false;
    }
}
