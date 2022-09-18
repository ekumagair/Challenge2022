using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomDePlateia : MonoBehaviour
{
    public enum Causa
    {
        Nulo,
        PrimeiroDanoJogador,
        PrimeiroInimigoMorto,
        TemSuficienteParaGirar,
        Venceu,
        Perdeu
    }

    public Causa causa;
    public int quantasVezes = 1;
    public AudioClip[] clips;

    AudioSource audioSource;
    GameObject jogador;
    Invector.vHealthController vida;
    bool tocou = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jogador = GameObject.FindGameObjectWithTag("Player");
        vida = jogador.GetComponent<Invector.vHealthController>();
        tocou = false;
    }

    void Update()
    {
        if (causa != Causa.Nulo)
        {
            switch (causa)
            {
                case Causa.PrimeiroDanoJogador:

                    if (vida.currentHealth < 95)
                    {
                        TocarSom();
                    }
                    break;

                case Causa.PrimeiroInimigoMorto:

                    if (StaticClass.inimigosMortos > 0)
                    {
                        TocarSom();
                    }
                    break;

                case Causa.TemSuficienteParaGirar:

                    if (StaticClass.inimigosMortos > 0 && StaticClass.inimigosMortos >= Jogador.inimigosMortosHabilidadeObjetivo)
                    {
                        TocarSom();
                    }
                    break;

                case Causa.Venceu:

                    if (StaticClass.estadoDeJogo == 1)
                    {
                        TocarSom();
                    }
                    break;

                case Causa.Perdeu:

                    if (StaticClass.estadoDeJogo == -1)
                    {
                        TocarSom();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    void TocarSom()
    {
        if (tocou == false)
        {
            tocou = true;
            quantasVezes--;

            int qualClip = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[qualClip], 1);

            if(StaticClass.debug == true)
            {
                Debug.Log("Tocou o som " + qualClip.ToString() + " (" + clips.Length.ToString() + " no total)");
            }

            if(quantasVezes <= 0)
            {
                causa = Causa.Nulo;
            }
            else
            {
                StartCoroutine(Resetar(clips[qualClip].length));
            }
        }
    }

    public IEnumerator Resetar(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log(gameObject + " pode tocar de novo.");
        tocou = false;
    }
}
