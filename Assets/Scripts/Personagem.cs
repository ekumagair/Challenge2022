using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    // Script usado por todos os personagens.
    // Se "jogador" = true, significa que este personagem é o jogador. Senão, é um inimigo.

    public bool jogador = false;
    public ParticleSystem rastroDeAtaque;
    public GameObject particulaDano;
    public GameObject particulaBlock;
    public GameObject particulaDeCura;
    public GameObject audioSource;
    public GameObject destruirAoMorrer;
    public GameObject localPes;
    public GameObject spine;
    public GameObject ragdoll;
    public int faseInfinitaValor = 0;
    public AudioClip[] clipDano;
    public AudioClip[] clipBlock;

    Animator animator;
    GameObject particulaDeCuraAtual = null;
    Invector.vHealthController vida;

    void Start()
    {
        animator = GetComponent<Animator>();
        vida = GetComponent<Invector.vHealthController>();
    }

    private void Awake()
    {
        if (jogador == false)
        {
            StaticClass.inimigosVivos++;

            if (StaticClass.debug == true)
            {
                Debug.Log("INIMIGOS VIVOS: " + StaticClass.inimigosVivos);
            }
        }
    }

    private void Update()
    {
        // Caso o personagem caia do cenário, leva ele de volta para cima.
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }

        if (particulaDeCura != null)
        {
            if (vida.inHealthRecovery == true && particulaDeCuraAtual == null)
            {
                particulaDeCuraAtual = Instantiate(particulaDeCura, transform.position + (transform.up * 1.5f), transform.rotation);

                ParticleSystem part = particulaDeCuraAtual.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule partMain = part.main;
                partMain.startDelay = vida.healthRecoveryDelay;


                particulaDeCuraAtual.transform.parent = gameObject.transform;
                particulaDeCuraAtual.transform.forward = Vector3.up;
            }
            if (vida.inHealthRecovery == false && particulaDeCuraAtual != null)
            {
                Destroy(particulaDeCuraAtual);
            }
        }
    }

    public void CriarParticulaDano()
    {
        if (animator == null || animator.GetBool("IsBlocking") == false)
        {
            Instantiate(particulaDano, transform.position + transform.up, transform.rotation);
            SomDano();
        }
        else
        {
            Instantiate(particulaBlock, transform.position + transform.up, transform.rotation);
            SomBlock();
        }

        if(GetComponent<Jogador>() != null && jogador == true)
        {
            ScreenShakeJogador();
        }

        if(particulaDeCuraAtual != null)
        {
            Destroy(particulaDeCuraAtual);
        }
    }

    public void MatouInimigo()
    {
        CriarParticulaDano();

        if(destruirAoMorrer != null)
        {
            // Destroi um objeto que faz parte do personagem. Usado, por exemplo, pelo gladiador, para deletar seu machado quando ele morre.
            Destroy(destruirAoMorrer);
        }

        if (jogador == false)
        {
            StaticClass.inimigosMortos++;
            StaticClass.pontosDeDificuldade += faseInfinitaValor;

            if (Jogador.girando == false)
            {
                Jogador.inimigosMortosHabilidade++;
            }

            if (StaticClass.debug == true)
            {
                Debug.Log("INIMIGOS MORTOS: " + StaticClass.inimigosMortos);
                Debug.Log("Porcentagem da fase: " + Mathf.RoundToInt(((float) StaticClass.inimigosMortos / (float) StaticClass.totalDeInimigos) * 100f).ToString());
            }
        }

        StartCoroutine(DestruirComponentes());
    }

    public void SomDano()
    {
        var snd = Instantiate(audioSource, transform.position + transform.up, transform.rotation);
        snd.GetComponent<AudioSource>().clip = clipDano[Random.Range(0, clipDano.Length)];
        snd.GetComponent<AudioSource>().PlayOneShot(snd.GetComponent<AudioSource>().clip, 1);
    }

    public void SomBlock()
    {
        var snd2 = Instantiate(audioSource, transform.position + transform.up, transform.rotation);
        snd2.GetComponent<AudioSource>().clip = clipBlock[Random.Range(0, clipBlock.Length)];
        snd2.GetComponent<AudioSource>().PlayOneShot(snd2.GetComponent<AudioSource>().clip, 1);
    }

    public void ScreenShakeJogador()
    {
        // Chacoalhar a câmera do jogador.
        GetComponent<Jogador>().HitShake();
    }

    public void RastroDeAtaque()
    {
        if (rastroDeAtaque != null)
        {
            rastroDeAtaque.Play();
        }
    }

    IEnumerator DestruirComponentes()
    {
        if (ragdoll == null)
        {
            yield return new WaitForSeconds(0.2f);

            if (GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>() != null)
            {
                Destroy(GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>());
            }
            if (GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Motor>() != null)
            {
                Destroy(GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Motor>());
            }
            if (GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Animator>() != null)
            {
                Destroy(GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Animator>());
            }
            if (GetComponent<Invector.vMelee.vMeleeAttackObject>() != null)
            {
                Destroy(GetComponent<Invector.vMelee.vMeleeAttackObject>());
            }
            if (GetComponent<Rigidbody>() != null)
            {
                Destroy(GetComponent<Rigidbody>());
            }
            if (GetComponent<Collider>() != null)
            {
                Destroy(GetComponent<Collider>());
            }
        }
        else
        {
            var rag = Instantiate(ragdoll, transform.position, transform.rotation);
            rag.GetComponent<Animator>().Play("Big_From_Front");

            if(StaticClass.debug)
            {
                Debug.Log(gameObject + " estava tocando a animação " + GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
            }

            yield return new WaitForSeconds(0.01f);

            Destroy(rag.GetComponent<Animator>());

            Destroy(gameObject);
        }

        if(StaticClass.faseAtual == 6)
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
    }
}
