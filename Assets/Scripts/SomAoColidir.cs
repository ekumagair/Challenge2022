using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomAoColidir : MonoBehaviour
{
    // Este script faz o objeto emitir um som aleatório ao colidir com algo sólido.

    public bool ativado = true;
    public AudioClip[] som;
    AudioSource _as;

    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ativado)
        {
            _as.PlayOneShot(som[Random.Range(0, som.Length)]);
        }
    }
}
