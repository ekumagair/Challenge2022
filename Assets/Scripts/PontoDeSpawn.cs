using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoDeSpawn : MonoBehaviour
{
    public GameObject[] tochas;

    public void DestaqueTochas()
    {
        for (int i = 0; i < tochas.Length; i++)
        {
            if (tochas[i].GetComponent<Tocha>() != null)
            {
                tochas[i].GetComponent<Tocha>().destaque = 5f;
            }
        }
    }
}
