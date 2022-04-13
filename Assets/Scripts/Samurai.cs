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

    float distanciaLimite = 5;
    public GameObject alvo;
    NavMeshAgent agente;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        alvo = GameObject.FindGameObjectWithTag("Player");
        morto = false;
        atacando = false;

        ResetarTempoDeAtaque();
        tempoAteAtacar += 2f;
        distanciaLimite = Random.Range(8f, 13f);
    }

    void Update()
    {
        if (morto == false && atacando == false && alvo != null)
        {
            if (Mathf.Abs(Vector3.Distance(alvo.transform.position, transform.position)) > distanciaLimite)
            {
                agente.destination = alvo.transform.position;
            }
            else
            {
                agente.destination = transform.position;
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
        }
    }

    IEnumerator Atacar()
    {
        atacando = true;
        ResetarTempoDeAtaque();

        var flecha = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        flecha.GetComponent<Projetil>().ignorar = "Enemy";
        flecha.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(0.9f);

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
    }
}
