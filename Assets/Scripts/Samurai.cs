using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Samurai : MonoBehaviour
{
    // O samurai atira flechas no jogador de longe. Ele não é muito ágil, para ser justo com o jogador que só tem armas corpo a corpo.

    public bool destruirAoMorrer = false;
    public float tempoAteAtacarMultiplicador = 1.0f;
    bool morto = false;

    float tempoAteAtacar = 5;
    bool atacando = false;
    bool iniciouAtaque = false;
    public GameObject projetil;
    public GameObject projetilModelo;

    bool levandoDano = false;
    bool levandoDanoParar = false;

    float distanciaLimite = 5;
    public GameObject alvo;
    NavMeshAgent agente;
    Animator animator;

    // distanciaLimite = Qual distância o samurai fica para atacar o jogador com projéteis.

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        alvo = GameObject.FindGameObjectWithTag("Player");
        morto = false;
        atacando = false;
        iniciouAtaque = false;
        levandoDano = false;
        levandoDanoParar = false;

        ResetarTempoDeAtaque();
        tempoAteAtacar += 2f;
        distanciaLimite = Random.Range(8f, 13f);

        if (projetilModelo != null)
        {
            projetilModelo.SetActive(false);
        }
    }

    void Update()
    {
        if (morto == false && atacando == false && alvo != null && levandoDanoParar == false)
        {
            if (Mathf.Abs(Vector3.Distance(alvo.transform.position, transform.position)) > distanciaLimite || levandoDano)
            {
                // Se não está levando dano, vai para o jogador. Se não, se afasta dele.
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
                // Ficar parado.
                agente.destination = transform.position;
            }

            // Parâmetros do animator.
            if(agente.velocity.magnitude > 0)
            {
                animator.SetBool("Movendo", true);
            }
            else
            {
                animator.SetBool("Movendo", false);
            }

            /*
            transform.LookAt(alvo.transform.position); Protótipo

            Vector3 resetar = transform.rotation.eulerAngles;
            resetar.x = 0;
            resetar.z = 0;

            transform.rotation = Quaternion.Euler(resetar);
            */

            // Olhar para o alvo gradualmente.
            Vector3 dir = alvo.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            tempoAteAtacar -= Time.deltaTime;

            if(tempoAteAtacar <= 0 && atacando == false && iniciouAtaque == false)
            {
                StartCoroutine(Atacar());
            }
        }
        else
        {
            // Ficar parado.
            agente.destination = transform.position;
            animator.SetBool("Movendo", false);
        }

        animator.SetBool("Morto", morto);
        animator.SetBool("Fugindo", levandoDano);
    }

    IEnumerator Atacar()
    {
        iniciouAtaque = true;
        atacando = true;
        animator.Play("Fire-Straight");

        if(projetilModelo != null)
        {
            projetilModelo.SetActive(true);
        }

        yield return new WaitForSeconds(0.4f);

        var flecha = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        flecha.GetComponent<Projetil>().ignorar = "Enemy";
        flecha.transform.rotation = transform.rotation;

        if (projetilModelo != null)
        {
            projetilModelo.SetActive(false);
        }

        yield return new WaitForSeconds(0.8f);

        ResetarTempoDeAtaque();
        atacando = false;
        iniciouAtaque = false;
    }

    public void ResetarTempoDeAtaque()
    {
        tempoAteAtacar = Random.Range(3f, 5f) * tempoAteAtacarMultiplicador;
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

    public void LevarDano()
    {
        animator.Play("BlockBreak");
        StartCoroutine(LevarDanoCoroutine());
    }

    IEnumerator LevarDanoCoroutine()
    {
        // Se afasta por alguns segundos sem atacar, depois volta ao normal.
        levandoDanoParar = true;

        yield return new WaitForSeconds(0.5f);

        levandoDanoParar = false;
        levandoDano = true;

        yield return new WaitForSeconds(3f);

        levandoDano = false;
    }
}
