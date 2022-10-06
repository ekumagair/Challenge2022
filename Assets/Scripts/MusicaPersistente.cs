using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaPersistente : MonoBehaviour
{
    public enum idMusica
    {
        Titulo
    }

    public idMusica qualMusica = 0;

    AudioSource _as;

    private void Start()
    {
        _as = GetComponent<AudioSource>();

        switch (qualMusica)
        {
            case idMusica.Titulo:
                _as.time = StaticClass.musicaMenuTempo;
                break;

            default:
                break;
        }

        _as.Play();
    }

    private void Update()
    {
        switch (qualMusica)
        {
            case idMusica.Titulo:
                StaticClass.musicaMenuTempo += Time.deltaTime;
                break;

            default:
                break;
        }
    }
}
