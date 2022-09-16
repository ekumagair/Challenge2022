using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 1;
    public string ignorar = "";
    public int dano = 20;
    public int efeitoAoAtingir = 0;
    public bool podeSerRefletido = false;
    public GameObject particulaImpacto;
    public GameObject particulaRastro;
    public GameObject modelo;
    public GameObject audioSource;
    public AudioClip[] clipAtingir;

    bool atingiu = false;
    Rigidbody _rb;

    // ignorar = Objetos com esta tag não são atingidos pelo projétil.

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
                Invector.vHealthController h = collision.gameObject.GetComponent<Invector.vHealthController>();

                if (h.isImmortal == false)
                {
                    // Se este ataque for letal, não prende o projétil no alvo.
                    if (h.currentHealth - dano <= 0 && efeitoAoAtingir == 1)
                    {
                        efeitoAoAtingir = 2;
                    }

                    // Causar dano.
                    h.AddHealth(dano * -1);
                }
            }

            ParticulaDeImpacto();
            SomDeImpacto();

            Atingiu(collision.gameObject);
        }
    }

    void ParticulaDeImpacto()
    {
        // Partícula de impacto.
        if (particulaImpacto != null)
        {
            Instantiate(particulaImpacto, transform.position, transform.rotation);
        }
    }

    void SomDeImpacto()
    {
        // Som de impacto.
        if (audioSource != null)
        {
            var snd = Instantiate(audioSource, transform.position + transform.up, transform.rotation);
            snd.GetComponent<AudioSource>().clip = clipAtingir[Random.Range(0, clipAtingir.Length)];
            snd.GetComponent<AudioSource>().PlayOneShot(snd.GetComponent<AudioSource>().clip, 1);
        }
    }

    void Atingiu(GameObject atingido)
    {
        if(modelo != null)
        {
            // Parar de girar modelo.
            if(modelo.GetComponent<Invector.vRotateObject>() != null)
            {
                modelo.GetComponent<Invector.vRotateObject>().enabled = false;
            }
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
            StartCoroutine(Atingiu1(atingido));
        }
        else if (efeitoAoAtingir == 2)
        {
            // Ativar gravidade.
            StartCoroutine(Atingiu2(atingido));
        }

        atingiu = true;
    }

    IEnumerator Atingiu1(GameObject atingido)
    {
        dano = 0;
        Destroy(GetComponent<Collider>());
        Destroy(_rb);

        if(particulaRastro != null)
        {
            particulaRastro.GetComponent<ParticleSystem>().Stop();
        }

        yield return new WaitForSeconds(0.015f);

        velocidade = 0;

        if (atingido.GetComponent<Personagem>() == null)
        {
            transform.parent = atingido.transform;
        }
        else
        {
            transform.parent = atingido.GetComponent<Personagem>().spine.transform;
            transform.position = atingido.transform.position + Vector3.up;
        }

        yield return new WaitForSeconds(15f);

        Destroy(gameObject);
    }

    IEnumerator Atingiu2(GameObject atingido)
    {
        _rb.constraints = RigidbodyConstraints.None;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        //_rb.velocity = (-transform.forward * (velocidade / 3)) + transform.up;
        _rb.velocity = (-transform.forward * 5) + transform.up;
        dano = 0;
        velocidade = 0;

        if (particulaRastro != null)
        {
            particulaRastro.GetComponent<ParticleSystem>().Stop();
        }
        if (atingido.GetComponent<Personagem>() != null)
        {
            Instantiate(atingido.GetComponent<Personagem>().particulaDano, atingido.transform.position, atingido.transform.rotation);
            atingido.GetComponent<Personagem>().SomDano();
        }

        yield return new WaitForSeconds(0.2f);

        if (GetComponent<SomAoColidir>() != null)
        {
            GetComponent<SomAoColidir>().ativado = true;
        }

        yield return new WaitForSeconds(15f);

        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "RefletirProjeteis" && podeSerRefletido == true && ignorar != "Player" && atingiu == false)
        {
            if (Jogador.armaEquipada == 0)
            {
                ignorar = "Player";

                // Faz o projétil inverter seu sentido, independente do ângulo do jogador.
                transform.forward *= -1f;

                // (ALTERNATIVO) Faz o projétil adotar o ângulo do jogador. É mais difícil de acertar quem atirou o projétil.
                //transform.forward = GameObject.FindGameObjectWithTag("Player").transform.forward;

                velocidade *= 1.5f;
                dano *= 4;

                if (GetComponent<BoxCollider>() != null)
                {
                    GetComponent<BoxCollider>().size *= 1.3f;
                }

                atingiu = false;
            }
            else if (Jogador.armaEquipada == 1)
            {
                efeitoAoAtingir = 2;
                Atingiu(other.gameObject);
            }

            ParticulaDeImpacto();
            SomDeImpacto();
        }
    }
}
