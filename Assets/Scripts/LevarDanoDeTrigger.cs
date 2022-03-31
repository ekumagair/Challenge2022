using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevarDanoDeTrigger : MonoBehaviour
{
    Invector.vHealthController vida;
    GameObject atingiu;
    
    void Awake()
    {
        vida = gameObject.GetComponent<Invector.vHealthController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TriggerDano" && other.gameObject.GetComponent<TriggerDano>() != null && other.gameObject != atingiu && Vector3.Distance(gameObject.transform.position, other.transform.position) < 2)
        {
            Debug.Log(Vector3.Distance(gameObject.transform.position, other.transform.position));
            vida.AddHealth(other.gameObject.GetComponent<TriggerDano>().dano * -1);
            atingiu = other.gameObject;
        }
    }
    
}
