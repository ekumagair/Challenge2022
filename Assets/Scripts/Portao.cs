using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portao : MonoBehaviour
{
    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
        StartCoroutine(Tocar());
    }

    IEnumerator Tocar()
    {
        yield return new WaitForSeconds(1f);

        anim.Play("Take 001");
    }
}
