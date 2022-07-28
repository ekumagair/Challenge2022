using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public bool jogador = false;
    public ParticleSystem rastroDeAtaque;
    public GameObject particulaDano;
    public GameObject particulaBlock;
    public GameObject audioSource;
    public GameObject destruirAoMorrer;
    public GameObject localPes;
    public AudioClip[] clipDano;
    public AudioClip[] clipBlock;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
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
    }

    public void MatouInimigo()
    {
        CriarParticulaDano();

        if(destruirAoMorrer != null)
        {
            Destroy(destruirAoMorrer);
        }

        if (jogador == false && Jogador.girando == false)
        {
            StaticClass.inimigosMortos++;
            Jogador.inimigosMortosHabilidade++;
            Debug.Log("INIMIGOS MORTOS: " + StaticClass.inimigosMortos);
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
}
