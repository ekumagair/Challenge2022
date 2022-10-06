using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaFase : MonoBehaviour
{
    AudioSource _as;

    void Start()
    {
        _as = GetComponent<AudioSource>();
        StartCoroutine(Tocar());
    }

    IEnumerator Tocar()
    {
        yield return new WaitForSeconds(0.01f);
        _as.Play();
    }

    void Update()
    {
        if (_as.volume > 0)
        {
            if (StaticClass.estadoDeJogo == 1 || StaticClass.estadoDeJogo == -1)
            {
                _as.volume -= Time.deltaTime * 2f;
            }
        }
        if(_as.volume < 0)
        {
            _as.volume = 0;
        }
    }
}
