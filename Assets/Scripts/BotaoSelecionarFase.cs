using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotaoSelecionarFase : MonoBehaviour
{
    public int fase = 1;
    Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if(fase <= StaticClass.faseDesbloqueada)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }
}
