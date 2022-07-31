using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MudarCorSeVidaBaixa : MonoBehaviour
{
    // A imagem que tem este script brilha uma certa cor quando a vida do objeto especificado est� baixa.

    public GameObject qualObjeto;
    public int limiteDeVida = 25;
    public Color corPadrao = new Color(1, 1, 1, 1);

    public float[] multiplicador = new float[4];
    // 0 = R
    // 1 = G
    // 2 = B

    Invector.vHealthController vidaDoObjeto;
    Image img;
    float[] novaCor = new float[4];

    void Start()
    {
        img = GetComponent<Image>();
        vidaDoObjeto = qualObjeto.GetComponent<Invector.vHealthController>();

        for (int i = 0; i < 3; i++)
        {
            novaCor[i] = 0f;
        }
    }

    void Update()
    {
        // S� muda a cor se o jogo n�o est� pausado.
        if (StaticClass.estadoDeJogo != 2)
        {
            if (vidaDoObjeto.currentHealth < limiteDeVida)
            {
                for (int i = 0; i < 4; i++)
                {
                    novaCor[i] += Time.deltaTime * multiplicador[i];

                    if (novaCor[i] > 1 && multiplicador[i] > 0)
                    {
                        novaCor[i] = 0.4f;
                    }
                }

                img.color = new Color(novaCor[0], novaCor[1], novaCor[2], 1);
            }
            else
            {
                img.color = corPadrao;
            }
        }
    }
}
