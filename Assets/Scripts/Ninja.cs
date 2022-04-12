using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ninja : MonoBehaviour
{
    public bool destruirAoMorrer = false;
    bool morto = false;

    bool atacando = false;
    public GameObject projetil;

    public GameObject destino;
    public GameObject[] destinos;
    NavMeshAgent agente;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        morto = false;
        atacando = false;

        destinos = GameObject.FindGameObjectsWithTag("Destino");
        destinos[0] = GameObject.FindGameObjectWithTag("Player");
        EscolherDestino(0);
    }

    void Update()
    {
        if (morto == false)
        {
            if (atacando == false)
            {
                agente.destination = destino.transform.position;
            }
            else
            {
                agente.destination = transform.position;
            }

            if (destino.tag == "Destino")
            {
                if (agente.remainingDistance < 1)
                {
                    EscolherDestino(0);
                }
            }
            else if (destino.tag == "Player")
            {
                if (agente.remainingDistance < 8 && atacando == false && Vector3.Distance(gameObject.transform.position, destino.transform.position) < 9)
                {
                    StartCoroutine(Atacar());
                }
            }

            transform.LookAt(destino.transform.position);

            Vector3 resetar = transform.rotation.eulerAngles;
            resetar.x = 0;
            resetar.z = 0;

            transform.rotation = Quaternion.Euler(resetar);
        }
    }

    public void EscolherDestino(int min)
    {
        destino = destinos[Random.Range(min, destinos.Length)];
    }

    IEnumerator Atacar()
    {
        atacando = true;

        yield return new WaitForSeconds(0.15f);

        var tiro = Instantiate(projetil, transform.position + transform.up + transform.forward, transform.rotation);
        tiro.GetComponent<Projetil>().ignorar = "Enemy";
        tiro.transform.rotation = transform.rotation;

        yield return new WaitForSeconds(2f);

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
    }
}
