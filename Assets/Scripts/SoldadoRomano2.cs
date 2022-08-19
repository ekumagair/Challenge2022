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
    bool atacando = false;
    bool girando = false;
    Animator animator;
    Personagem personagem;
    Invector.vHealthController vida;
    Invector.vCharacterController.AI.vSimpleMeleeAI_Motor melee;

    void Start()
    {
        animator = GetComponent<Animator>();
        vida = GetComponent<Invector.vHealthController>();
        personagem = GetComponent<Personagem>();
        melee = GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>();
        timerLanca = 8f;
        atacando = false;
        girando = false;
        fezAtaqueGiratorio = false;
    }

    void Update()
    {
        if (!atacando && !girando && !melee.isAttacking)
        {
            timerLanca -= Time.deltaTime;

            if (timerLanca <= 0)
            {
                StartCoroutine(Jogar());
            }

            if (vida.currentHealth <= 40 && !fezAtaqueGiratorio && timerLanca >= 1f)
            {
                StartCoroutine(AtaqueGiratorio());
            }
        }
    }

    public void LevouDano()
    {
        timerLanca = 5f;
        lancaModelo.SetActive(true);
        atacando = false;
        StopCoroutine(Jogar());
    }

    IEnumerator Jogar()
    {
        atacando = true;
        timerLanca = 5f;
        animator.Play("Throw", 0);

        yield return new WaitForSeconds(0.2f);

        lancaModelo.SetActive(false);
        var l = Instantiate(lanca, transform.position + transform.up, transform.rotation);
        l.GetComponent<Projetil>().ignorar = gameObject.tag;

        yield return new WaitForSeconds(0.6f);

        lancaModelo.SetActive(true);
        atacando = false;
    }

    IEnumerator AtaqueGiratorio()
    {
        GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>().OnEnableAttack();
        StopCoroutine(Jogar());

        atacando = true;
        girando = true;
        fezAtaqueGiratorio = true;
        timerLanca = 3f;
        animator.Play("MoveAttack1", 0);
        lancaModelo.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        personagem.RastroDeAtaque();
        var dg = Instantiate(danoGiratorio, transform.position, transform.rotation);
        dg.transform.parent = gameObject.transform;

        yield return new WaitForSeconds(1.2f);

        atacando = false;
        girando = false;
        GetComponent<Invector.vCharacterController.AI.vSimpleMeleeAI_Controller>().FinishAttack();
    }
}
