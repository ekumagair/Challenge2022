using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldadoRomano2 : MonoBehaviour
{
    public GameObject lanca;
    public GameObject lancaModelo;
    public GameObject danoGiratorio;
    public bool fezAtaqueGiratorio = false;

    float timerLanca = 8f;
    public float timerLancaMultiplicador = 1.0f;
    public float lancaVelocidadeMultiplicador = 1.0f;
    bool atacando = false;
    bool girando = false;
    Animator animator;
    Personagem personagem;
    Invector.vHealthController vida;
    Invector.vCharacterController.AI.vSimpleMeleeAI_Controller controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        vida = GetComponent<Invector.vHealthController>();
        personagem = GetComponent<Personagem>();
        controller = GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>();
        timerLanca = 15f;
        atacando = false;
        girando = false;
        fezAtaqueGiratorio = false;
    }

    void Update()
    {
        if (!atacando && !girando && !controller.isAttacking && !vida.isDead && !controller.isBlocking && !controller.isCrouching && personagem.cambalearTimer <= 0)
        {
            timerLanca -= Time.deltaTime;

            if (timerLanca <= 0)
            {
                StartCoroutine(Jogar());
            }

            if (vida.currentHealth <= 40 && !fezAtaqueGiratorio && timerLanca > 1f)
            {
                StartCoroutine(AtaqueGiratorio());
            }
        }
    }

    public void LevouDano()
    {
        timerLanca = 5f * timerLancaMultiplicador;
        lancaModelo.SetActive(true);
        atacando = false;
        StopCoroutine(Jogar());
    }

    public void Morrer()
    {
        StopCoroutine(Jogar());
        StopCoroutine(AtaqueGiratorio());
    }

    IEnumerator Jogar()
    {
        if (controller != null)
        {
            controller.OnEnableAttack();
        }

        vida.isImmortal = false;
        atacando = true;
        timerLanca = 5f * timerLancaMultiplicador;
        animator.Play("Throw", 0);

        yield return new WaitForSeconds(0.2f);

        lancaModelo.SetActive(false);
        var l = Instantiate(lanca, transform.position + transform.up, transform.rotation);
        l.GetComponent<Projetil>().ignorar = gameObject.tag;
        l.GetComponent<Projetil>().velocidade *= lancaVelocidadeMultiplicador;

        yield return new WaitForSeconds(0.6f);

        lancaModelo.SetActive(true);
        atacando = false;

        if (controller != null)
        {
            controller.OnDisableAttack();
        }
    }

    IEnumerator AtaqueGiratorio()
    {
        if (controller != null)
        {
            controller.OnEnableAttack();
        }

        vida.isImmortal = true;
        StopCoroutine(Jogar());

        atacando = true;
        girando = true;
        fezAtaqueGiratorio = true;
        timerLanca = 3f * timerLancaMultiplicador;
        animator.Play("MoveAttack1", 0);
        lancaModelo.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        personagem.RastroDeAtaque();
        var dg = Instantiate(danoGiratorio, transform.position, transform.rotation);
        dg.transform.parent = gameObject.transform;

        yield return new WaitForSeconds(1.25f);

        atacando = false;
        girando = false;
        vida.isImmortal = false;

        if (controller != null)
        {
            controller.OnDisableAttack();
        }
    }
}
