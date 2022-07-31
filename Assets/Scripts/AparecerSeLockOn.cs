using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AparecerSeLockOn : MonoBehaviour
{
    // Uma imagem que tem este script é escondida se o jogador não está travando a mira em um inimigo.

    Image img;
    Invector.vCharacterController.vLockOn lockOn;

    void Start()
    {
        img = GetComponent<Image>();
        lockOn = GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vCharacterController.vLockOn>();
    }

    void Update()
    {
        if(lockOn.isLockingOn)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
