using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MudarCorSeVidaBaixa : MonoBehaviour
{
    // A imagem que tem este script brilha uma certa cor quando a vida do objeto especificado está baixa.

    public GameObject qualObjeto;
    public int limiteDeVida = 25;
    public Color corPadrao = new Color(1, 1, 1, 1);
    public bool animado = false;

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
        // Só muda a cor se o jogo não está pausado.
        if (StaticClass.estadoDeJogo != 2)
        {
            if (vidaDoObjeto.currentHealth < limiteDeVida)
            {
                if (animado == true)
                {
                    // Brilhar
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
                    // Mudar de cor sem animação
                    img.color = new Color(1f * multiplicador[0], 1f * multiplicador[1], 1f * multiplicador[2], 1f * multiplicador[3]);
                }
            }
            else
            {
                img.color = corPadrao;
            }
        }
    }
}
