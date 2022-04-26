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

    public bool podeFicarInvisivel = true;
    bool invisivel = false;
    public Material[] meshDefaultMat;
    public MeshRenderer[] mesh;
    public Material materialInvisivel;

    public GameObject destino;
    public GameObject[] destinos;
    NavMeshAgent agente;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        morto = false;
        atacando = false;
        invisivel = false;

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
            /*
            transform.LookAt(destino.transform.position);

            Vector3 resetar = transform.rotation.eulerAngles;
            resetar.x = 0;
            resetar.z = 0;

            transform.rotation = Quaternion.Euler(resetar);
            */

            Vector3 dir = destino.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    public void EscolherDestino(int min)
    {
        destino = destinos[Random.Range(min, destinos.Length)];

        if(podeFicarInvisivel == true)
        {
            if (invisivel == false)
            {
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

    void FicarTransparente()
    {
        int qualMesh = 0;

        foreach (MeshRenderer mr in mesh)
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
}
