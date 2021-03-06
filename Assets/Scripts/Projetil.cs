using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 1;
    public string ignorar = "";
    public int dano = 20;
    public int efeitoAoAtingir = 0;
    public GameObject particulaImpacto;
    public GameObject particulaRastro;
    public GameObject audioSource;
    public AudioClip[] clipAtingir;

    bool atingiu = false;
    Rigidbody _rb;

    // ignorar = Objetos com esta tag n?o s?o atingidos pelo proj?til.

    private void Start()
    {
        atingiu = false;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ir para frente.
        transform.position += velocidade * transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != ignorar && atingiu == false)
        {
            // Se o objeto atingido tem vida, causa dano nele.
            if(collision.gameObject.GetComponent<Invector.vHealthController>() != null)
            {
                collision.gameObject.GetComponent<Invector.vHealthController>().AddHealth(dano * -1);
            }

            // Part?cula de impacto.
            if (particulaImpacto != null)
            {
                Instantiate(particulaImpacto, transform.position, transform.rotation);
            }

            // Som de impacto.
            if (audioSource != null)
            {
                var snd = Instantiate(audioSource, transform.position + transform.up, transform.rotation);
                snd.GetComponent<AudioSource>().clip = clipAtingir[Random.Range(0, clipAtingir.Length)];
                snd.GetComponent<AudioSource>().PlayOneShot(snd.GetComponent<AudioSource>().clip, 1);
            }

            // Efeitos visuais de impacto.
            if (efeitoAoAtingir == 0)
            {
                // Apenas destruir.
                Destroy(gameObject);
            }
            else if (efeitoAoAtingir == 1)
            {
                // Prender no objeto atingido.
                StartCoroutine(Atingiu1(collision.gameObject));
            }
            else if (efeitoAoAtingir == 2)
            {
                // Ativar gravidade.
                StartCoroutine(Atingiu2());
            }

            atingiu = true;
        }
    }

    IEnumerator Atingiu1(GameObject atingido)
    {
        dano = 0;
        Destroy(GetComponent<Collider>());
        Destroy(_rb);

        if(particulaRastro != null)
        {
            Destroy(particulaRastro);
        }

        yield return new WaitForSeconds(0.01f);

        velocidade = 0;
        transform.parent = atingido.transform;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    IEnumerator Atingiu2()
    {
        _rb.constraints = RigidbodyConstraints.None;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.velocity = (-transform.forward * (velocidade / 3)) + transform.up;
        dano = 0;
        velocidade = 0;

        if (particulaRastro != null)
        {
            Destroy(particulaRastro);
        }

        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
