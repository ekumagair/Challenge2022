using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 1;
    public string ignorar = "";
    public int dano = 20;
    public GameObject efeitoImpacto;

    void Update()
    {
        transform.position += velocidade * transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != ignorar)
        {
            if(collision.gameObject.GetComponent<Invector.vHealthController>() != null)
            {
                collision.gameObject.GetComponent<Invector.vHealthController>().AddHealth(dano * -1);
            }

            if (efeitoImpacto != null)
            {
                Instantiate(efeitoImpacto, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
