using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int faseDesbloqueada;
    public int inimigosMortosRecorde;
    public float sensibilidadeMouse;
    public float volumeGlobal;
    public float volumeHitSound;
    public float shakeMult;

    public UserData()
    {
        faseDesbloqueada = StaticClass.faseDesbloqueada;
        inimigosMortosRecorde = StaticClass.inimigosMortosRecorde;
        sensibilidadeMouse = StaticClass.sensibilidadeMouse;
        volumeGlobal = StaticClass.volumeGlobal;
        volumeHitSound = StaticClass.volumeHitSound;
        shakeMult = StaticClass.shakeMult;
    }
}
