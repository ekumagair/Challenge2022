using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tocha : MonoBehaviour
{
    public GameObject objFogo;
    public GameObject objFumaca;

    public ParticleSystem.MainModule fogo;
    public ParticleSystem.MainModule fumaca;
    public Light luz;

    public float destaque;

    void Start()
    {
        fogo = objFogo.GetComponent<ParticleSystem>().main;
        fumaca = objFumaca.GetComponent<ParticleSystem>().main;
    }

    void Update()
    {
        if(destaque > 0)
        {
            destaque -= Time.deltaTime;

            if(fogo.startSizeMultiplier < 2.0f)
            {
                fogo.startSizeMultiplier += Time.deltaTime * 2f;
            }
            if(luz.range < 30f)
            {
                luz.range += Time.deltaTime * 2f;
            }
        }

        if (destaque < 0)
        {
            destaque = 0;
        }

        if(destaque == 0)
        {
            if (fogo.startSizeMultiplier > 1.0f)
            {
                fogo.startSizeMultiplier -= Time.deltaTime * 2f;
            }
            if (luz.range > 15f)
            {
                luz.range -= Time.deltaTime * 2f;
            }
        }
    }
}
