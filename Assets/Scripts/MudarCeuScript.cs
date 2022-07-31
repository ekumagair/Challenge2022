using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudarCeuScript : MonoBehaviour
{
    // Este script muda o material do céu e a cor e intensidade da luz ambiente do mapa, dependendo da fase.

    public int qualFase = 1;
    public Color cor = new Color(1, 1, 1, 1);
    public float intensidade;
    public Material skyboxMaterial;

    Light luz;

    void Start()
    {
        if(qualFase == StaticClass.faseAtual)
        {
            luz = GameObject.Find("Directional Light").GetComponent<Light>();
            luz.color = cor;
            luz.intensity = intensidade;

            if(skyboxMaterial != null)
            {
                RenderSettings.skybox = skyboxMaterial;
            }
        }

        Destroy(gameObject);
    }
}
