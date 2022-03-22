using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevarDanoDeTrigger : MonoBehaviour
{
    Invector.vHealthController vida;
    GameObject atingiu;

    void Awake()
    {
        vida = GetComponent<Invector.vHealthController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TriggerDano" && other.gameObject.GetComponent<TriggerDano>() != null && other.gameObject != atingiu)
        {
            vida.AddHealth(other.gameObject.GetComponent<TriggerDano>().dano * -1);
            atingiu = other.gameObject;
        }
    }
}
