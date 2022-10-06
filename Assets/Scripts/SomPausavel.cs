using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomPausavel : MonoBehaviour
{
    AudioSource _as;

    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (StaticClass.estadoDeJogo == 2)
        {
            _as.Pause();
        }
        else
        {
            _as.UnPause();
        }
    }
}
